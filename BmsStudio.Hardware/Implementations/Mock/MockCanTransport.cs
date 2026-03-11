using BmsStudio.Core.Entities;
using BmsStudio.HardwareAbstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Hardware.Implementations.Mock
{
    internal class MockCanTransport : ICanTransport
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
}
