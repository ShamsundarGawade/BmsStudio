using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.HardwareAbstractions.Interfaces
{
    public interface IUdsClient
    {
        Task<bool> StartSession();
    }
}
