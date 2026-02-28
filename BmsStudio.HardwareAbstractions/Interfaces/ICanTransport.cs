
namespace BmsStudio.HardwareAbstractions.Interfaces;
public interface ICanTransport { Task SendAsync(byte[] frame); Task<byte[]> ReceiveAsync(); }
