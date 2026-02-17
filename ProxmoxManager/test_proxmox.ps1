
param (
    [string]$Url,
    [string]$Password
)

if ([string]::IsNullOrEmpty($Url)) {
    Write-Host "Please provide a URL."
    exit
}

# Trust all certificates
add-type @"
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    public class TrustAllCertsPolicy : ICertificatePolicy {
        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem) {
            return true;
        }
    }
"@
[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy

try {
    # 1. Authenticate
    $LoginUrl = "$Url/api2/json/access/ticket"
    $Body = @{
        username = "root@pam"
        password = $Password
    }
    
    Write-Host "Testing connection to: $LoginUrl"
    $Response = Invoke-RestMethod -Uri $LoginUrl -Method Post -Body $Body -ErrorAction Stop

    Write-Host "Success!" -ForegroundColor Green
    Write-Host "Ticket: $($Response.data.ticket.Substring(0, 20))..."
    Write-Host "CSRF: $($Response.data.CSRFPreventionToken)"
    
    $Ticket = "PVEAuthCookie=$($Response.data.ticket)"
    $Csrf = $Response.data.CSRFPreventionToken
        
    $Headers = @{
        "Cookie"              = $Ticket
        "CSRFPreventionToken" = $Csrf
    }

    # 2. Get Cluster Resources (God Mode)
    $ClusterUrl = "$Url/api2/json/cluster/resources"
    Write-Host "Fetching cluster resources from: $ClusterUrl" -ForegroundColor Magenta
    
    $ClusterResponse = Invoke-RestMethod -Uri $ClusterUrl -Method Get -Headers $Headers -ErrorAction Stop
    $Resources = $ClusterResponse.data
    
    $VmCount = ($Resources | Where-Object { $_.type -eq 'qemu' }).Count
    $CtCount = ($Resources | Where-Object { $_.type -eq 'lxc' }).Count
    $NodeCount = ($Resources | Where-Object { $_.type -eq 'node' }).Count
    
    Write-Host "Cluster Summary:" -ForegroundColor Green
    Write-Host "  Nodes: $NodeCount"
    Write-Host "  VMs:   $VmCount"
    Write-Host "  CTs:   $CtCount"
    
    if ($VmCount -gt 0) {
        Write-Host "First 5 VMs:" -ForegroundColor Yellow
        $Resources | Where-Object { $_.type -eq 'qemu' } | Select-Object -First 5 | Format-Table id, node, status, name -AutoSize | Out-String | Write-Host
    }

    # 3. Test IPv4 Retrieval for Running VMs (Guest Agent)
    $RunningVms = $Resources | Where-Object { $_.type -eq 'qemu' -and $_.status -eq 'running' } | Select-Object -First 3
    
    if ($RunningVms) {
        Write-Host "Checking IP for up to 3 running VMs..." -ForegroundColor Cyan
        foreach ($vm in $RunningVms) {
            Write-Host "  Querying Agent for VM $($vm.name) ($($vm.vmid))..." -NoNewline
            $AgentUrl = "$Url/api2/json/nodes/$($vm.node)/qemu/$($vm.vmid)/agent/network-get-interfaces"
            try {
                $AgentResponse = Invoke-RestMethod -Uri $AgentUrl -Method Get -Headers $Headers -ErrorAction Stop
                
                $Interfaces = $AgentResponse.data.result
                $FoundIp = $false
                foreach ($iface in $Interfaces) {
                    foreach ($ip in $iface.'ip-addresses') {
                        if ($ip.'ip-address-type' -eq 'ipv4' -and $ip.'ip-address' -ne '127.0.0.1') {
                            Write-Host " Found IPv4: $($ip.'ip-address')" -ForegroundColor Green
                            $FoundIp = $true
                        }
                    }
                }
                if (-not $FoundIp) { Write-Host " No IPv4 found." -ForegroundColor DarkGray }
            }
            catch {
                Write-Host " Agent Error: $($_.Exception.Message)" -ForegroundColor DarkGray
            }
        }
    }
    else {
        Write-Host "No running VMs to test Guest Agent." -ForegroundColor DarkGray
    }

}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        Write-Host "Response Body:"
        try {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            Write-Host $reader.ReadToEnd()
        }
        catch {}
    }
}
