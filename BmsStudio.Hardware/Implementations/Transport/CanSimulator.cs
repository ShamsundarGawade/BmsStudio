
using BmsStudio.Core.Entities;
using BmsStudio.HardwareAbstractions.Interfaces;

namespace BmsStudio.Hardware.Implementations;
public class CanSimulator : ICanTransport
{
    public event Action<CanMessage>? MessageReceived;

    public void StartReading()
    {
        throw new NotImplementedException();
    }

    public void StopReading()
    {
        throw new NotImplementedException();
    }
}
