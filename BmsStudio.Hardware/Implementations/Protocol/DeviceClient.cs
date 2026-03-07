
using BmsStudio.HardwareAbstractions.Interfaces;
using BmsStudio.HardwareAbstractions.Models;

namespace BmsStudio.Hardware.Implementations;
public class DeviceClient : IDeviceClient
{

    private bool _isConnected;
    //private TPCANHandle _canHandle = TPCANHandle.PCAN_USBBUS1; // Adjust as needed
    public async Task<bool> ConnectAsync()
    {
        try
        {
            // Actual hardware connection logic here
            // For example, open CAN port, check baud rate, etc.
            _isConnected = await OpenCanPortAsync();
            return _isConnected;
        }
        catch (Exception ex)
        {
            _isConnected = false;
            // Log or handle error
            return false;
        }
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

    private async Task<bool> OpenCanPortAsync()
    {
        // Open the CAN port asynchronously
        return await Task.Run(() =>
        {
            // Initialize the CAN channel at 500 kbit/s (adjust as needed)
            //TPCANStatus status = PCANBasic.Initialize(
            //    _canHandle,
            //    TPCANBaudrate.PCAN_BAUD_500K);

            //if (status == TPCANStatus.PCAN_ERROR_OK)
            //{
            //    // Port opened successfully
            //    return true;
            //}
            //else
            //{
            //    // Handle error (optional: log status)
            //    return false;
            //}
            return true; // Simulate successful connection for this example
        });
    }
}
