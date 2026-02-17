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
        
        // Bindings
        txtUrl.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Url), false, DataSourceUpdateMode.OnPropertyChanged);
        txtUsername.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Username), false, DataSourceUpdateMode.OnPropertyChanged);
        txtPassword.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Password), false, DataSourceUpdateMode.OnPropertyChanged);
        txtRealm.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Realm), false, DataSourceUpdateMode.OnPropertyChanged);
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
