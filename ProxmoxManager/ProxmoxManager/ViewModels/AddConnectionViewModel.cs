using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProxmoxManager.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProxmoxManager.ViewModels;

public partial class AddConnectionViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IProxmoxService _proxmoxService;
    private readonly ConnectionService _connectionService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPasswordAuth))]
    private AuthType selectedAuthType = AuthType.Password;
    
    public bool IsPasswordAuth => SelectedAuthType == AuthType.Password;

    [ObservableProperty]
    private string url = "https://";

    [ObservableProperty]
    private string username = "root";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string realm = "pam";

    [ObservableProperty]
    private string statusMessage = "";

    public Action? OnRequestClose;

    public AddConnectionViewModel(IAuthService authService, 
                                  IProxmoxService proxmoxService, 
                                  ConnectionService connectionService)
    {
        _authService = authService;
        _proxmoxService = proxmoxService;
        _connectionService = connectionService;
    }

    [RelayCommand]
    public async Task TestAndSaveAsync()
    {
        StatusMessage = "Connecting...";
        try
        {
            // Authenticate with selected type
            await _proxmoxService.AuthenticateAsync(Url, Username, Password, Realm, SelectedAuthType);
            
            await _connectionService.SaveConnectionAsync(Url);
            _authService.SaveCredentials(Url, Username, Password, Realm, SelectedAuthType);
            
            StatusMessage = "Success!";
            await Task.Delay(500);
            OnRequestClose?.Invoke();
        }
        catch (HttpRequestException ex)
        {
            StatusMessage = $"Http Error: {ex.Message} (Status: {ex.StatusCode})";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }
}
