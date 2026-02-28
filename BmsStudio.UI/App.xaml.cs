
using BmsStudio.Hardware;
using BmsStudio.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace BmsStudio.UI;
public partial class App : System.Windows.Application
{
    private IHost? _host;
    protected override async void OnStartup(StartupEventArgs e)
    {
        _host = Host.CreateDefaultBuilder().ConfigureServices((ctx, services) =>
        {
            services.AddInfrastructure();
            services.AddHardware();
            services.AddSingleton<MainWindow>();
        }).Build();
        await _host.StartAsync();
        var main = _host.Services.GetRequiredService<MainWindow>();
        main.Show();
        base.OnStartup(e);
    }
}
