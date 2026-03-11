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
        public Task<byte[]> ReceiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(byte[] frame)
        {
            throw new NotImplementedException();
        }
    }
}
