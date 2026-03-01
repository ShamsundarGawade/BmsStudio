using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Application.DTOs
{
    public class ConnectionStatusDto
    {
        public bool IsConnected { get; set; }
        public string Message { get; set; }
    }
}
