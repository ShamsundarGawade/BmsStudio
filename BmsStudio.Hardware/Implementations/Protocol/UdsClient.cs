
using BmsStudio.HardwareAbstractions.Interfaces;

namespace BmsStudio.Hardware.Implementations;
public class UdsClient : IUdsClient
{
    private readonly ICanTransport _transport;
    public UdsClient(ICanTransport transport)
    {
        _transport = transport;
    }
    public async Task<bool> StartSession()
    {
        await _transport.SendAsync(new byte[] { 0x10, 0x03 });
        return true;
    }
}
