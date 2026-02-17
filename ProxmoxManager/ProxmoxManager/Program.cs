using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProxmoxManager.Services;
using ProxmoxManager.ViewModels;
using System;
using System.Windows.Forms;

namespace ProxmoxManager;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var host = CreateHostBuilder().Build();
        ServiceProvider = host.Services;

        Application.Run(ServiceProvider.GetRequiredService<MainForm>());
    }

    public static IServiceProvider ServiceProvider { get; private set; }

    static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                services.AddHttpClient<IProxmoxService, ProxmoxService>()
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    });
                services.AddSingleton<IAuthService, AuthService>();
                services.AddSingleton<ConnectionService>();
                services.AddSingleton<MainViewModel>();
                services.AddTransient<MainForm>();
                services.AddTransient<AddConnectionViewModel>();
                services.AddTransient<Forms.AddConnectionForm>();
            });
    }
}