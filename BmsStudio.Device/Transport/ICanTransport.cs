
namespace BmsStudio.Device.Transport;
public interface ICanTransport { Task SendAsync(byte[] frame); Task<byte[]> ReceiveAsync(); }
