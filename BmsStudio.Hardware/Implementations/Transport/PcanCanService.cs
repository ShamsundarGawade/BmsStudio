using BmsStudio.Core.Entities;
using BmsStudio.HardwareAbstractions.Interfaces;
using Peak.Can.Basic;
using Peak.Can.Xcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Hardware.Implementations.Transport
{
    public class PcanCanService : ICanTransport
    {
        private readonly ushort _channel = PCANBasic.PCAN_USBBUS1;
        private readonly TPCANBaudrate _baudrate = TPCANBaudrate.PCAN_BAUD_500K;

        public bool Connect()
        {
            var result = PCANBasic.Initialize(_channel, _baudrate);
            return result == TPCANStatus.PCAN_ERROR_OK;
        }

        public void Disconnect()
        {
            PCANBasic.Uninitialize(_channel);
        }

        public bool Send(uint id, byte[] data)
        {
            var msg = new TPCANMsg
            {
                ID = id,
                LEN = (byte)data.Length,
                MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD,
                DATA = data
            };

            var result = PCANBasic.Write(_channel, ref msg);
            return result == TPCANStatus.PCAN_ERROR_OK;
        }

        public bool TryReceive(out uint id, out byte[] data)
        {
            id = 0;
            data = new byte[8];

            var result = PCANBasic.Read(_channel, out TPCANMsg msg, out _);

            if (result == TPCANStatus.PCAN_ERROR_OK)
            {
                id = msg.ID;
                data = msg.DATA;
                return true;
            }

            return false;
        }
    }
}
