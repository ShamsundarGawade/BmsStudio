
namespace BmsStudio.Device.Transport;
public class CanSimulator : ICanTransport {
    private readonly Random _rand=new();
    public Task SendAsync(byte[] frame)=>Task.CompletedTask;
    public Task<byte[]> ReceiveAsync(){var b=new byte[8];_rand.NextBytes(b);return Task.FromResult(b);}
}
