using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Application.DTOs
{
    public class BatteryStatusDto
    {
        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Temperature { get; internal set; }
    }
}
