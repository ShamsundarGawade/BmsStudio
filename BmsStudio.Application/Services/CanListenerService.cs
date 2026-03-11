using BmsStudio.Application.Interfaces;
using BmsStudio.Core.Entities;
using BmsStudio.HardwareAbstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Application.Services
{
    public class CanListenerService : ICanListenerService
    {
        private readonly ICanTransport _canService;

        public CanListenerService(ICanTransport canService)
        {
            _canService = canService;

            _canService.MessageReceived += OnMessageReceived;
        }

        public event Action<CanMessage>? MessageReceived;

        public void Start()
        {
            _canService.StartReading();
        }

        public void Stop()
        {
            _canService.StopReading();
        }

        private void OnMessageReceived(CanMessage msg)
        {
            Console.WriteLine($"ID:{msg.Id} Data:{BitConverter.ToString(msg.Data)}");
        }
    }
}
