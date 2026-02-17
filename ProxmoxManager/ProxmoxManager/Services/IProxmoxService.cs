using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProxmoxManager.Services;

public interface IProxmoxService
{
    Task<ProxmoxAuthResult> AuthenticateAsync(string url, string username, string password, string realm, AuthType type = AuthType.Password);
    Task<List<string>> GetNodesAsync(string url, ProxmoxAuthResult auth);
    Task<List<Models.ProxmoxVm>> GetVmsAsync(string url, string node, ProxmoxAuthResult auth);
    Task<List<Models.ProxmoxCt>> GetCtsAsync(string url, string node, ProxmoxAuthResult auth);
    Task<List<Models.ProxmoxClusterResource>> GetClusterResourcesAsync(string url, ProxmoxAuthResult auth);
    Task<string> GetVmIpAsync(string url, string node, int vmid, ProxmoxAuthResult auth);
    Task<string> GetCtIpAsync(string url, string node, int vmid, ProxmoxAuthResult auth);
    Task<(bool Success, string Error)> IsValidConnection(string url);
}
