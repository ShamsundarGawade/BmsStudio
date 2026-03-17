using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.HardwareAbstractions.Models
{
    public class PcanConfig
    {
        public ushort Channel { get; set; }
        public uint Baudrate { get; set; }
        public uint RequestId { get; set; }
        public uint ResponseId { get; set; }
    }
}
