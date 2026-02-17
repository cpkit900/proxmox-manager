namespace ProxmoxManager.Services;

public enum AuthType
{
    Password,
    ApiToken
}

public class ProxmoxAuthResult
{
    public string Ticket { get; set; } = "";
    public string CSRFPreventionToken { get; set; } = "";
    
    public string ApiToken { get; set; } = ""; // Format: User@Realm!TokenId=Secret
    public AuthType Type { get; set; } = AuthType.Password;
}
