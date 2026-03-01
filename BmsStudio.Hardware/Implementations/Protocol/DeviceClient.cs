
using BmsStudio.HardwareAbstractions.Interfaces;
using BmsStudio.HardwareAbstractions.Models;

namespace BmsStudio.Hardware.Implementations;
public class DeviceClient : IDeviceClient
{

    private bool _isConnected;

    public async Task ConnectAsync()
    {
        // Simulate connection logic
        await Task.Delay(500);
        _isConnected = true;

        Console.WriteLine("Device connected.");
    }

    public async Task DisconnectAsync()
    {
        await Task.Delay(200);
        _isConnected = false;

        Console.WriteLine("Device disconnected.");
    }

    public async Task<HardwareData> ReadAsync()
    {
        if (!_isConnected)
            throw new InvalidOperationException("Device not connected.");

        // Simulate hardware read
        await Task.Delay(300);

        return new HardwareData
        {
            Timestamp = DateTime.UtcNow,
            Voltage = 48.5,
            Current = 12.3,
            Temperature = 35.6
        };
    }
}
