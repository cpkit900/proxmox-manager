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
            // Attempt to authenticate directly
            // IsValidConnection is sometimes too strict (e.g. failing on 401 for /version)
            await _proxmoxService.AuthenticateAsync(Url, Username, Password, Realm);
            
            await _connectionService.SaveConnectionAsync(Url);
            _authService.SaveCredentials(Url, Username, Password, Realm);
            
            StatusMessage = "Success!";
            await Task.Delay(500);
            OnRequestClose?.Invoke();
        }
        catch (HttpRequestException ex)
        {
            StatusMessage = $"Connection Failed: {ex.Message}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }
}
