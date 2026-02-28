
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using BmsStudio.Application.Interfaces;
using BmsStudio.Infrastructure.Services;
using BmsStudio.HardwareAbstractions.Interfaces;
using BmsStudio.Hardware.Implementations;

namespace BmsStudio.UI;
public partial class App : System.Windows.Application
{
    private IHost? _host;
    protected override async void OnStartup(StartupEventArgs e)
    {
        _host = Host.CreateDefaultBuilder().ConfigureServices((ctx, services) =>
        {
            services.AddSingleton<ICanTransport, CanSimulator>();
            services.AddSingleton<IUdsClient, UdsClient>();
            services.AddSingleton<IBmsConnectionService, BmsConnectionService>();
            services.AddSingleton<ITelemetryService, TelemetryService>();
            services.AddSingleton<MainWindow>();
        }).Build();
        await _host.StartAsync();
        var main = _host.Services.GetRequiredService<MainWindow>();
        main.Show();
        base.OnStartup(e);
    }
}
