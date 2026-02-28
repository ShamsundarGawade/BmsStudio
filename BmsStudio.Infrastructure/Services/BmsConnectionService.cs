
using BmsStudio.Application.Interfaces;
using BmsStudio.Core.Models;
using BmsStudio.Core.Enums;
using BmsStudio.Device.Protocol;
namespace BmsStudio.Infrastructure.Services;
public class BmsConnectionService : IBmsConnectionService
{
    private readonly UdsClient _uds;
    public ConnectionState State { get; private set; } = ConnectionState.Disconnected;
    public BmsConnectionService(UdsClient uds) { _uds = uds; }
    public async Task<bool> ConnectAsync(CanBaudRate baudRate)
    {
        State = ConnectionState.Connecting;
        var ok = await _uds.StartSession();
        State = ok ? ConnectionState.Connected : ConnectionState.Error; return ok;
    }
    public Task DisconnectAsync()
    {
        State = ConnectionState.Disconnected;
        return Task.CompletedTask;
    }
    public Task<BmsDevice> ReadDeviceInfoAsync() => Task.FromResult(new BmsDevice { FirmwareVersion = "1.0.0", HardwareVersion = "HW-A", SerialNumber = "SIM123" });
}
