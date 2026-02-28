
using BmsStudio.Application.Interfaces;
using BmsStudio.Core.Entities;
using BmsStudio.Core.Enums;
using BmsStudio.HardwareAbstractions.Interfaces;
namespace BmsStudio.Infrastructure.Services;
public class BmsConnectionService : IBmsConnectionService
{
    private readonly IUdsClient _udsClient;
    public ConnectionState State { get; private set; } = ConnectionState.Disconnected;
    public BmsConnectionService(IUdsClient uds)
    {
        _udsClient = uds;
    }
    public async Task<bool> ConnectAsync(CanBaudRate baudRate)
    {
        State = ConnectionState.Connecting;
        var ok = await _udsClient.StartSession();
        State = ok ? ConnectionState.Connected : ConnectionState.Error; return ok;
    }
    public Task DisconnectAsync()
    {
        State = ConnectionState.Disconnected;
        return Task.CompletedTask;
    }
    public Task<BmsDevice> ReadDeviceInfoAsync() => Task.FromResult(new BmsDevice { FirmwareVersion = "1.0.0", HardwareVersion = "HW-A", SerialNumber = "SIM123" });
}
