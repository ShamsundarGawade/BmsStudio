using BmsStudio.Application.Interfaces;
using BmsStudio.Application.Services;
using BmsStudio.HardwareAbstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BmsStudio.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IBmsConnectionService, BmsConnectionService>();
            services.AddSingleton<ICanListenerService, CanListenerService>();

            return services;
        }
    }
}
