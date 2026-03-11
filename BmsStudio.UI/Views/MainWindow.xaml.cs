
using BmsStudio.Hardware.Implementations;
using BmsStudio.UI.ViewModels;
using System.Windows;
using BmsStudio.Application.Services;
using BmsStudio.Hardware.Implementations.Transport;
using BmsStudio.HardwareAbstractions.Interfaces;
namespace BmsStudio.UI;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Create dependencies manually

        // Create CAN hardware service
        ICanTransport canTransport = new PcanCanService();

        // Create CAN listener
        ICanListenerService canListenerService = new CanListenerService(canTransport);

        // Inject into DeviceClient
        var deviceClient = new DeviceClient(canListenerService);

        // Existing code
        var connectionService = new BmsConnectionService(deviceClient);

        // Inject into ViewModel
        DataContext = new MainViewModel(connectionService);
    }
}
