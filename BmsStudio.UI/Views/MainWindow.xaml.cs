
using BmsStudio.Hardware.Implementations;
using BmsStudio.UI.ViewModels;
using System.Windows;
using BmsStudio.Application.Services;
namespace BmsStudio.UI;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Create dependencies manually
        var deviceClient = new DeviceClient(); // or your actual implementation
        var connectionService = new BmsConnectionService(deviceClient);

        // Inject into ViewModel
        DataContext = new MainViewModel(connectionService);
    }
}
