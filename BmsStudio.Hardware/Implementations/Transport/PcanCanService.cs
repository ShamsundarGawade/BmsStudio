using BmsStudio.Core.Entities;
using BmsStudio.HardwareAbstractions.Interfaces;
using Peak.Can.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Hardware.Implementations.Transport
{
    public class PcanCanService : ICanTransport
    {
        // need to find how i can handle this TPCANHandle
        private readonly ushort _handle = PCANBasic.PCAN_USBBUS1;
        private bool _reading;

        public event Action<CanMessage>? MessageReceived;

        public void StartReading()
        {
            _reading = true;

            Task.Run(ReadLoop);
        }

        public void StopReading()
        {
            _reading = false;
        }

        private void ReadLoop()
        {
           // while (_reading)
           // {
                TPCANMsg msg;
                TPCANTimestamp timestamp;

                var status = PCANBasic.Read(_handle, out msg, out timestamp);

                if (status == TPCANStatus.PCAN_ERROR_OK)
                {
                    var canMessage = new CanMessage
                    {
                        Id = msg.ID,
                        Length = msg.LEN,
                        Data = msg.DATA,
                        Timestamp = DateTime.Now
                    };

                    MessageReceived?.Invoke(canMessage);
                }
            //}
        }
    }
}
