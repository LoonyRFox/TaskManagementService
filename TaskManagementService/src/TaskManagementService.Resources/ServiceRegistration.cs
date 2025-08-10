using Microsoft.Extensions.DependencyInjection;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Resources.Services;

namespace TaskManagementService.Resources
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddResourcesInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ITranslator, Translator>();

            return services;
        }
    }
}
