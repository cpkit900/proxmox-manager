using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ProxmoxManager.Models;

namespace ProxmoxManager.Services;

public class ProxmoxService : IProxmoxService
{
    private readonly HttpClient _httpClient;

    public ProxmoxService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Ignore SSL errors for local proxmox instances with self-signed certs
        // Note: In production, this should be configurable or handled more securely
    }

    private void SetHeaders(HttpRequestMessage request, ProxmoxAuthResult auth)
    {
        if (auth.Type == AuthType.ApiToken)
        {
            // PVEAPIToken=USER@REALM!TOKENID=UUID
            // Use TryAddWithoutValidation to bypass strict format checks for custom schemes
            request.Headers.TryAddWithoutValidation("Authorization", $"PVEAPIToken={auth.ApiToken}");
        }
        else
        {
            request.Headers.Add("CSRFPreventionToken", auth.CSRFPreventionToken);
            request.Headers.Add("Cookie", $"PVEAuthCookie={auth.Ticket}");
        }
    }

    public async Task<ProxmoxAuthResult> AuthenticateAsync(string url, string username, string password, string realm, AuthType type = AuthType.Password)
    {
        url = url.TrimEnd('/');
        
        if (type == AuthType.ApiToken)
        {
            // For API Token, we just validate connectivity. 
            // Username = User@Realm!TokenId
            // Password = Secret
            var token = $"{username}={password}";
            var auth = new ProxmoxAuthResult { ApiToken = token, Type = AuthType.ApiToken };
            
            // Validate
            var request = new HttpRequestMessage(HttpMethod.Get, $"{url}/api2/json/version");
            SetHeaders(request, auth);
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            return auth;
        }

        var loginUrl = $"{url}/api2/json/access/ticket";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("username", $"{username}@{realm}"),
            new KeyValuePair<string, string>("password", password)
        });

        var loginResponse = await _httpClient.PostAsync(loginUrl, content);
        loginResponse.EnsureSuccessStatusCode();

        var json = await loginResponse.Content.ReadFromJsonAsync<JsonObject>();
        var data = json["data"];
        
        return new ProxmoxAuthResult
        {
            Ticket = data["ticket"].ToString(),
            CSRFPreventionToken = data["CSRFPreventionToken"].ToString(),
            Type = AuthType.Password
        };
    }

    public async Task<List<string>> GetNodesAsync(string url, ProxmoxAuthResult auth)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{url.TrimEnd('/')}/api2/json/nodes");
        SetHeaders(request, auth);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadFromJsonAsync<JsonObject>();
        var nodes = new List<string>();
        foreach (var node in json["data"].AsArray())
        {
            nodes.Add(node["node"].ToString());
        }
        return nodes;
    }

    public async Task<List<ProxmoxVm>> GetVmsAsync(string url, string node, ProxmoxAuthResult auth)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{url.TrimEnd('/')}/api2/json/nodes/{node}/qemu");
        SetHeaders(request, auth);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return new List<ProxmoxVm>();
        
        var json = await response.Content.ReadFromJsonAsync<JsonObject>();
        // Simple mapping, handling potentially missing fields locally if needed
        var vms = new List<ProxmoxVm>();
        foreach(var item in json["data"].AsArray())
        {
             vms.Add(new ProxmoxVm 
             {
                 Id = (int)item["vmid"],
                 Name = item["name"]?.ToString() ?? "Unknown",
                 Status = item["status"]?.ToString() ?? "unknown",
                 Pid = item["pid"] != null ? (int)item["pid"] : null
             });
        }
        return vms;
    }

    public async Task<List<ProxmoxCt>> GetCtsAsync(string url, string node, ProxmoxAuthResult auth)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{url.TrimEnd('/')}/api2/json/nodes/{node}/lxc");
        SetHeaders(request, auth);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return new List<ProxmoxCt>();
        
        var json = await response.Content.ReadFromJsonAsync<JsonObject>();
        var cts = new List<ProxmoxCt>();
        foreach(var item in json["data"].AsArray())
        {
             cts.Add(new ProxmoxCt 
             {
                 Id = (int)item["vmid"],
                 Name = item["name"]?.ToString() ?? "Unknown",
                 Status = item["status"]?.ToString() ?? "unknown",
                 Type = item["type"]?.ToString() ?? "lxc"
             });
        }
        return cts;
    }

    public async Task<List<ProxmoxClusterResource>> GetClusterResourcesAsync(string url, ProxmoxAuthResult auth)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{url.TrimEnd('/')}/api2/json/cluster/resources");
        SetHeaders(request, auth);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return new List<ProxmoxClusterResource>();
        
        var json = await response.Content.ReadFromJsonAsync<JsonObject>();
        var resources = new List<ProxmoxClusterResource>();
        
        foreach(var item in json["data"].AsArray())
        {
             resources.Add(new ProxmoxClusterResource 
             {
                 Id = item["id"]?.ToString() ?? "",
                 Type = item["type"]?.ToString() ?? "",
                 VmId = item["vmid"] != null ? (int)item["vmid"] : null,
                 Node = item["node"]?.ToString() ?? "",
                 Status = item["status"]?.ToString() ?? "unknown",
                 Name = item["name"]?.ToString() ?? "Unknown"
             });
        }
        return resources;
    }

    public async Task<string> GetVmIpAsync(string url, string node, int vmid, ProxmoxAuthResult auth)
    {
        // Try QEMU Guest Agent
        try 
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{url.TrimEnd('/')}/api2/json/nodes/{node}/qemu/{vmid}/agent/network-get-interfaces");
            SetHeaders(request, auth);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return "No Agent";

            var json = await response.Content.ReadFromJsonAsync<JsonObject>();
            var result = json["data"]?["result"]?.AsArray();
            
            if (result == null) return "No Data";

            foreach (var iface in result)
            {
                var ips = iface["ip-addresses"]?.AsArray();
                if (ips != null)
                {
                    foreach (var ip in ips)
                    {
                        var type = ip["ip-address-type"]?.ToString();
                        var addr = ip["ip-address"]?.ToString();
                        
                        // Return first non-loopback IPv4
                        if (type == "ipv4" && addr != "127.0.0.1")
                        {
                            return addr;
                        }
                    }
                }
            }
        }
        catch 
        { 
            // Agent might not be running
            return "Agent Error";
        }
        return "Not Found";
    }

    public async Task<string> GetCtIpAsync(string url, string node, int vmid, ProxmoxAuthResult auth)
    {
        // LXC Interfaces
        try 
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{url.TrimEnd('/')}/api2/json/nodes/{node}/lxc/{vmid}/interfaces");
            SetHeaders(request, auth);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return "No Data";

            var json = await response.Content.ReadFromJsonAsync<JsonObject>();
            var result = json["data"]?.AsArray();
            
            if (result == null) return "No Data";

            foreach (var iface in result)
            {
                var name = iface["name"]?.ToString();
                if (name == "lo") continue; // Skip loopback

                var inet = iface["inet"]?.ToString(); // CIDR format usually, e.g. 192.168.1.50/24
                if (!string.IsNullOrEmpty(inet))
                {
                    // Basic parsing to remove CIDR suffix if present
                    var ip = inet.Split('/')[0];
                    return ip;
                }
            }
        }
        catch 
        { 
            return "Error";
        }
        return "Not Found";
    }

    public async Task<(bool Success, string Error)> IsValidConnection(string url)
    {
        try
        {
            // Ensure URL starts with http/https
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
               return (false, "URL must start with http:// or https://");
            }

            var response = await _httpClient.GetAsync($"{url.TrimEnd('/')}/api2/json/version");
            if (response.IsSuccessStatusCode)
            {
                return (true, "");
            }
            return (false, $"HTTP Error: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return (false, $"Connection Failed: {ex.Message}");
        }
    }
}
