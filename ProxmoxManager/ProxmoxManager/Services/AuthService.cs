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

    public void SaveCredentials(string url, string username, string password, string realm, AuthType authType)
    {
        using var cred = new Credential
        {
            Target = $"ProxmoxManager:{url}",
            // Store metadata in Username field: "Type|Username|Realm"
            Username = $"{authType}|{username}|{realm}",
            Password = password,
            Type = CredentialType.Generic,
            PersistanceType = PersistanceType.LocalComputer
        };
        cred.Save();
    }

    public (string Username, string Password, string Realm, AuthType Type)? GetCredentials(string url)
    {
        using var cred = new Credential { Target = $"ProxmoxManager:{url}" };
        if (cred.Load())
        {
            var parts = cred.Username.Split('|');
            
            // Legacy Format: "User|Realm" (Length 2) -> Assume Password
            if (parts.Length == 2)
            {
                return (parts[0], cred.Password, parts[1], AuthType.Password);
            }
            // New Format: "Type|User|Realm" (Length 3)
            else if (parts.Length == 3)
            {
                 if (Enum.TryParse<AuthType>(parts[0], out var type))
                 {
                     return (parts[1], cred.Password, parts[2], type);
                 }
            }
            
            // Fallback
            return (parts[0], cred.Password, "pam", AuthType.Password);
        }
        return null;
    }

    public void ClearCredentials(string url)
    {
        using var cred = new Credential { Target = $"ProxmoxManager:{url}" };
        cred.Delete();
    }
}
