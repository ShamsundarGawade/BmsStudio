using BmsStudio.HardwareAbstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.HardwareAbstractions.Interfaces
{
    public interface IDeviceClient
    {
        Task<bool> ConnectAsync();
        Task<HardwareData> ReadAsync();
        Task DisconnectAsync();
    }
}
