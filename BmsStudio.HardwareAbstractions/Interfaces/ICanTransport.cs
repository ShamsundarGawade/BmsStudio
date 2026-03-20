
using BmsStudio.Core.Entities;

namespace BmsStudio.HardwareAbstractions.Interfaces;
public interface ICanTransport
{
    bool Connect();

    bool Send(uint canId, byte[] data);

    bool TryReceive(out uint id, out byte[] data);

    void Disconnect();
}
