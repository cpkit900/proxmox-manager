using System.Threading.Tasks;

namespace ProxmoxManager.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string url, string username, string password, string realm);
    void SaveCredentials(string url, string username, string password, string realm);
    (string Username, string Password, string Realm)? GetCredentials(string url);
    void ClearCredentials(string url);
}
