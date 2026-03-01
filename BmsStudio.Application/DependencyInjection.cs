using BmsStudio.Application.Interfaces;
using BmsStudio.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BmsStudio.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IBmsConnectionService, BmsConnectionService>();

            return services;
        }
    }
}
