using CredentialManagement;
using System;
using System.Threading.Tasks;

namespace ProxmoxManager.Services;

public class AuthService : IAuthService
{
    // Placeholder for actual API login logic
    public async Task<bool> LoginAsync(string url, string username, string password, string realm)
    {
        // TODO: Implement actual Proxmox API authentication
        await Task.Delay(500); // Simulate network request
        return true; 
    }

    public void SaveCredentials(string url, string username, string password, string realm)
    {
        using var cred = new Credential
        {
            Target = $"ProxmoxManager:{url}",
            Username = $"{username}|{realm}",
            Password = password,
            Type = CredentialType.Generic,
            PersistanceType = PersistanceType.LocalComputer
        };
        cred.Save();
    }

    public (string Username, string Password, string Realm)? GetCredentials(string url)
    {
        using var cred = new Credential { Target = $"ProxmoxManager:{url}" };
        if (cred.Load())
        {
            var parts = cred.Username.Split('|');
            var user = parts[0];
            var realm = parts.Length > 1 ? parts[1] : "pam";
            return (user, cred.Password, realm);
        }
        return null;
    }

    public void ClearCredentials(string url)
    {
        using var cred = new Credential { Target = $"ProxmoxManager:{url}" };
        cred.Delete();
    }
}
