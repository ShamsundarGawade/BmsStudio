
using BmsStudio.Core.Entities;

namespace BmsStudio.HardwareAbstractions.Interfaces;
public interface ICanTransport
{
    //Task SendAsync(byte[] frame);
    //Task<byte[]> ReceiveAsync();

    //void Open();
    //void Close();
    //void Send(byte[] data);
    //event Action<byte[]> MessageReceived;

    void StartReading();

    void StopReading();

    event Action<CanMessage>? MessageReceived;
}
