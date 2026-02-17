
using System.Text.Json.Serialization;

namespace ProxmoxManager.Models;

public class ProxmoxVm
{
    [JsonPropertyName("vmid")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "";
    
    // QEMU specific
    [JsonPropertyName("pid")]
    public int? Pid { get; set; }

    public string Type => "qemu";
    
    public string IpAddress { get; set; } = "Unknown";
}

public class ProxmoxCt
{
    [JsonPropertyName("vmid")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "";
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = "lxc";
    
    public string IpAddress { get; set; } = "Unknown";
}

public class ProxmoxNode
{
    public string Name { get; set; } = "";
    public List<ProxmoxVm> Vms { get; set; } = new();
    public List<ProxmoxCt> Cts { get; set; } = new();
}
