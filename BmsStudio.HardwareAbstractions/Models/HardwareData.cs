using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.HardwareAbstractions.Models
{
    public class HardwareData
    {
        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Temperature { get; set; }
        public DateTime Timestamp { get; set; }
        // Or even raw bytes if needed
        //public byte[] RawFrame { get; set; }
    }
}
