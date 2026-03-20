using BmsStudio.HardwareAbstractions.Interfaces;
using BmsStudio.HardwareAbstractions.Models;
using Peak.Can.Basic;
using Peak.Can.Uds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Hardware.Implementations.Protocol.UDS
{
    public class PcanUdsService : IPcanUdsService
    {
        public void Initialize(PcanConfig config)
        {
            var result = UDSApi.Initialize(
                config.Channel,
                (TPUDSBaudrate)config.Baudrate);

            if (result != TPUDSStatus.PUDS_ERROR_OK)
            {
                throw new Exception($"UDS Init Failed: {result}");
            }

            // 🔴 IMPORTANT: Mapping
            //UDSApi.AddMapping(
            //    config.RequestId,
            //    config.ResponseId,
            //    0,
            //    0);
        }

        byte[] IPcanUdsService.SendRequest(byte[] request)
        {
            throw new NotImplementedException();
        }

        //public byte[] SendRequest(byte[] request)
        //{
        //    var writeResult = UDSApi.Write(request);

        //    if (writeResult != TPUDSStatus.PUDS_ERROR_OK)
        //        throw new Exception("UDS Write Failed");

        //    byte[] response = new byte[4096];

        //    var readResult = UDSApi.Read(response);

        //    if (readResult != TPUDSStatus.PUDS_ERROR_OK)
        //        throw new Exception("UDS Read Failed");

        //    return response;
        //}
    }
}
