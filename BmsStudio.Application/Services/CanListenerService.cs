using BmsStudio.Core.Entities;
using BmsStudio.HardwareAbstractions.Interfaces;

namespace BmsStudio.Application.Services
{
    public class CanListenerService : ICanListenerService
    {
        private readonly ICanTransport _transport;

        public event Action<uint, byte[]>? MessageReceived;
        //public event Action<CanMessage>? MessageReceived;

        public CanListenerService(ICanTransport transport)
        {
            _transport = transport;
        }


        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (_transport.TryReceive(out var id, out var data))
                    {
                        MessageReceived?.Invoke(id, data);
                    }
                }
            });
        }


    }
}
