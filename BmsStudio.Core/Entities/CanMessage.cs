using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Core.Entities
{
    public class CanMessage
    {
        public uint Id { get; set; }

        public byte Length { get; set; }

        public byte[] Data { get; set; } = new byte[8];

        public DateTime Timestamp { get; set; }
    }
}
