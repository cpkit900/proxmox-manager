using System.Text.Json.Serialization;

namespace ProxmoxManager.Models;

public class ProxmoxClusterResource
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("type")]
    public string Type { get; set; } = ""; // node, qemu, lxc

    [JsonPropertyName("vmid")]
    public int? VmId { get; set; }

    [JsonPropertyName("node")]
    public string Node { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = ""; // running, stopped

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    // Helper property
    public string IpAddress { get; set; } = "Unknown";
}
