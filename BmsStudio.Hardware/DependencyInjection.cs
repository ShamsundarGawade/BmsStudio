using BmsStudio.Hardware.Implementations;
using BmsStudio.Hardware.Implementations.Transport;
using BmsStudio.HardwareAbstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsStudio.Hardware
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHardware(this IServiceCollection services)
        {
            services.AddSingleton<IDeviceClient, DeviceClient>();
            services.AddSingleton<ICanTransport, PcanCanService>();

            return services;
        }
    }
}
