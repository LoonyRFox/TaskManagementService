using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagementServiceLoging.Application.Features.BackgroundServices;
using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Application.Validator;

namespace TaskManagementServiceLoging.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
           
            services.AddHostedService<CreateLogNotificationBackgroundService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<ILogIncomingMessageValidator, LogIncomingMessageValidator>();
            return services;
        }
    }
}
