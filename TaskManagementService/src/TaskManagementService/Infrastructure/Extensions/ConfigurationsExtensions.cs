using Microsoft.Extensions.Options;
using TaskManagementService.Application.Interfaces.Messaging;
using TaskManagementService.Messaging.Configurations;
using TaskManagementService.Persistence.Configurations;

namespace TaskManagementService.WebApi.Infrastructure.Extensions;

public static class ConfigurationsExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        // Регистрация конфигураций с интерфейсами
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox")); 
        services.Configure<KafkaConfig>(configuration.GetSection("Outbox:Kafka"));
        services.AddSingleton<IKafkaConfig>(sp => sp.GetRequiredService<IOptions<KafkaConfig>>().Value);
        return services;
    }
}