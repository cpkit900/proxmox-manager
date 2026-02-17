using Microsoft.Extensions.DependencyInjection;
using ProxmoxManager.ViewModels;

namespace ProxmoxManager;

public partial class MainForm : Form
{
    private readonly MainViewModel _viewModel;

    private readonly IServiceProvider _serviceProvider;

    public MainForm(MainViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;

        // Data Binding
        this.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Title));
        lblStatus.DataBindings.Add("Text", _viewModel, nameof(_viewModel.StatusMessage));
        
        dgvConnections.DataSource = _viewModel.Connections;
        
        btnSync.Click += async (s, e) => {
            btnSync.Enabled = false;
            await _viewModel.SyncConnectionsCommand.ExecuteAsync(null);
            btnSync.Enabled = true;
        };

        btnAdd.Click += (s, e) => {
            using var form = _serviceProvider.GetRequiredService<Forms.AddConnectionForm>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Trigger sync or refresh list
                _viewModel.SyncConnectionsCommand.ExecuteAsync(null);
            }
        };

        // Initial Sync
        this.Load += async (s, e) => await _viewModel.SyncConnectionsCommand.ExecuteAsync(null);

        dgvConnections.SelectionChanged += (s, e) => {
            tvDetails.Nodes.Clear();
            if (dgvConnections.SelectedRows.Count > 0)
            {
                var conn = dgvConnections.SelectedRows[0].DataBoundItem as Models.ProxmoxConnection;
                if (conn != null && conn.NodeDetails != null)
                {
                    foreach (var node in conn.NodeDetails)
                    {
                        var treeNode = tvDetails.Nodes.Add(node.Name);
                        treeNode.ImageKey = "server"; // Icons would be nice later
                        
                        // VMs
                        var vmsNode = treeNode.Nodes.Add($"VMs ({node.Vms.Count})");
                        foreach (var vm in node.Vms)
                        {
                            var ipInfo = vm.IpAddress != "Unknown" ? vm.IpAddress : "No IP";
                            var vmText = $"{vm.Id}: {vm.Name} ({vm.Status}) - {ipInfo}";
                            vmsNode.Nodes.Add(vmText);
                        }
                        
                        // CTs
                        var ctsNode = treeNode.Nodes.Add($"CTs ({node.Cts.Count})");
                        foreach (var ct in node.Cts)
                        {
                            var ipInfo = ct.IpAddress != "Unknown" ? ct.IpAddress : "No IP";
                            var ctText = $"{ct.Id}: {ct.Name} ({ct.Status}) - {ipInfo}";
                            ctsNode.Nodes.Add(ctText);
                        }
                        
                        treeNode.Expand();
                        vmsNode.Expand();
                    }
                }
            }
        };
    }
}
