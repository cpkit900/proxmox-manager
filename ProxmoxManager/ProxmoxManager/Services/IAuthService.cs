using System.Threading.Tasks;

namespace ProxmoxManager.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string url, string username, string password, string realm);
    void SaveCredentials(string url, string username, string password, string realm, AuthType authType);
    (string Username, string Password, string Realm, AuthType Type)? GetCredentials(string url);
    void ClearCredentials(string url);
}
