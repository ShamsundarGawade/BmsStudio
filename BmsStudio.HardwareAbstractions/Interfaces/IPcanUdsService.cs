using BmsStudio.HardwareAbstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.HardwareAbstractions.Interfaces
{
    public interface IPcanUdsService
    {
        void Initialize(PcanConfig config);

        byte[] SendRequest(byte[] request);
    }
}
