using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ProxmoxManager.Models;

public partial class ProxmoxConnection : ObservableObject
{
    [ObservableProperty]
    private string url;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string realm;

    [ObservableProperty]
    private string status = "Disconnected";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NodesList))]
    private List<string> nodes = new();

    [ObservableProperty]
    private int vmCount;

    [ObservableProperty]
    private int ctCount;

    [ObservableProperty]
    private List<ProxmoxNode> nodeDetails = new();

    public string DisplayName => $"{Username}@{Realm} on {Url}";

    public string NodesList => string.Join(", ", Nodes);
}
