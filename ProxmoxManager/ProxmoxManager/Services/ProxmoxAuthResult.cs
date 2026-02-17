namespace ProxmoxManager.Services;

public class ProxmoxAuthResult
{
    public string Ticket { get; set; } = "";
    public string CSRFPreventionToken { get; set; } = "";
}
