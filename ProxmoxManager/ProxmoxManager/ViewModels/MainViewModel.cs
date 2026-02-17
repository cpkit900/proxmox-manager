using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using ProxmoxManager.Models;
using ProxmoxManager.Services;

namespace ProxmoxManager.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "Proxmox Manager";

    private readonly IAuthService _authService;
    private readonly IProxmoxService _proxmoxService;
    private readonly ConnectionService _connectionService;

    public BindingList<ProxmoxConnection> Connections { get; } = new();

    [ObservableProperty]
    private string statusMessage = "Ready";

    public MainViewModel(IAuthService authService,
                         IProxmoxService proxmoxService,
                         ConnectionService connectionService)
    {
        _authService = authService;
        _proxmoxService = proxmoxService;
        _connectionService = connectionService;
    }

    [RelayCommand]
    public async Task SyncConnectionsAsync()
    {
        // Capture context to ensure modifications happen on UI thread
        var uiContext = SynchronizationContext.Current;
        
        void SafeUpdate(Action action)
        {
            if (uiContext != null)
                uiContext.Post(_ => action(), null);
            else
                action();
        }

        StatusMessage = "Syncing...";
        SafeUpdate(() => Connections.Clear());
        
        var urls = await _connectionService.LoadConnectionsAsync();
        foreach (var url in urls)
        {
            var conn = new ProxmoxConnection { Url = url };
            var creds = _authService.GetCredentials(url);

            if (creds != null)
            {
                conn.Username = creds.Value.Username;
                conn.Realm = creds.Value.Realm;
                
                try
                {
                    conn.Status = "Connecting...";
                    // Temporary update for status
                    SafeUpdate(() => {
                         if (!Connections.Contains(conn)) Connections.Add(conn); 
                    });

                    // Use stored AuthType
                    var auth = await _proxmoxService.AuthenticateAsync(url, creds.Value.Username, creds.Value.Password, creds.Value.Realm, creds.Value.Type);
                    conn.Status = "Connected";
                    
                    // 1. Fetch ALL resources (God Mode)
                    var allResources = await _proxmoxService.GetClusterResourcesAsync(url, auth);
                    
                    // 2. Filter Lists
                    var nodeList = allResources.Where(r => r.Type == "node").ToList();
                    var vmList = allResources.Where(r => r.Type == "qemu").ToList();
                    var ctList = allResources.Where(r => r.Type == "lxc").ToList();
                    
                    conn.Nodes = nodeList.Select(n => n.Node).ToList();
                    conn.VmCount = vmList.Count;
                    conn.CtCount = ctList.Count;
                    
                    // 3. Organise Tree Data
                    var treeData = new List<ProxmoxNode>();
                    foreach (var node in nodeList)
                    {
                        var pNode = new ProxmoxNode { Name = node.Node };
                        
                        // Map VMs to this node
                        foreach(var vm in vmList.Where(v => v.Node == node.Node))
                        {
                            var hasIp = false;
                            // Attempt IP fetch for running VMs (Async)
                            // Note: Doing this sequentially might be slow with many VMs. 
                            // For V1 we do it here. Ideal: Parallel.ForEach or generic Task.WhenAll
                            if (vm.Status == "running" && vm.VmId.HasValue)
                            {
                                 vm.IpAddress = await _proxmoxService.GetVmIpAsync(url, vm.Node, vm.VmId.Value, auth);
                            }

                            pNode.Vms.Add(new ProxmoxVm 
                            { 
                                Id = vm.VmId ?? 0, 
                                Name = vm.Name, 
                                Status = vm.Status,
                                IpAddress = vm.IpAddress
                            });
                        }

                        // Map CTs to this node
                        foreach(var ct in ctList.Where(c => c.Node == node.Node))
                        {
                            if (ct.Status == "running" && ct.VmId.HasValue)
                            {
                                 ct.IpAddress = await _proxmoxService.GetCtIpAsync(url, ct.Node, ct.VmId.Value, auth);
                            }

                            pNode.Cts.Add(new ProxmoxCt 
                            { 
                                Id = ct.VmId ?? 0, 
                                Name = ct.Name, 
                                Status = ct.Status,
                                IpAddress = ct.IpAddress // Pass the fetched IP
                            });
                        }
                        
                        treeData.Add(pNode);
                    }
                    
                    conn.NodeDetails = treeData;
                    conn.Status = $"Connected ({conn.Nodes.Count} nodes, {conn.VmCount} VMs, {conn.CtCount} CTs)";
                }
                catch (Exception ex)
                {
                    conn.Status = $"Error: {ex.Message}";
                }
            }
            else
            {
                conn.Status = "Credentials missing";
            }
            
            // Ensure final add if not already added or update
            SafeUpdate(() => {
                if (!Connections.Contains(conn)) 
                    Connections.Add(conn);
            });
        }
        StatusMessage = "Sync Complete";
    }
}
