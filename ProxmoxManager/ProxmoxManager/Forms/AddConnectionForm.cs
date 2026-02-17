using ProxmoxManager.ViewModels;
using System;
using System.Windows.Forms;

namespace ProxmoxManager.Forms;

public partial class AddConnectionForm : Form
{
    private readonly AddConnectionViewModel _viewModel;

    public AddConnectionForm(AddConnectionViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        
        // Setup Combo Box
        cmbAuthType.DataSource = Enum.GetValues(typeof(Services.AuthType));
        cmbAuthType.DataBindings.Add("SelectedItem", _viewModel, nameof(_viewModel.SelectedAuthType), false, DataSourceUpdateMode.OnPropertyChanged);

        // Bindings
        txtUrl.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Url), false, DataSourceUpdateMode.OnPropertyChanged);
        txtUsername.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Username), false, DataSourceUpdateMode.OnPropertyChanged);
        txtPassword.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Password), false, DataSourceUpdateMode.OnPropertyChanged);
        txtRealm.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Realm), false, DataSourceUpdateMode.OnPropertyChanged);
        
        // Realm visibility binding
        txtRealm.DataBindings.Add("Enabled", _viewModel, nameof(_viewModel.IsPasswordAuth));
        
        // Update Labels based on selection
        cmbAuthType.SelectedIndexChanged += (s, e) => {
            if (cmbAuthType.SelectedItem is Services.AuthType selected && selected == Services.AuthType.ApiToken) {
                _viewModel.SelectedAuthType = selected; // Force update ViewModel just in case
                label2.Text = "Token ID (User@Realm!Token):";
                label3.Text = "Secret:";
                label4.Visible = false;
                txtRealm.Visible = false;
            } else {
                if (cmbAuthType.SelectedItem is Services.AuthType sel) _viewModel.SelectedAuthType = sel;
                label2.Text = "Username:";
                label3.Text = "Password:";
                label4.Visible = true;
                txtRealm.Visible = true;
            }
        };

        lblStatus.DataBindings.Add("Text", _viewModel, nameof(_viewModel.StatusMessage));

        btnSave.Click += async (s, e) => {
            btnSave.Enabled = false;
            await _viewModel.TestAndSaveAsync();
            btnSave.Enabled = true;
        };

        _viewModel.OnRequestClose += () => {
            this.DialogResult = DialogResult.OK;
            this.Close();
        };
    }
}
