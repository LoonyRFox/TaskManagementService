using Microsoft.Extensions.Options;
using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Infrastructure.Confuguration;
using TaskManagementServiceLoging.Infrastructure.Messaging;


namespace TaskManagementServiceLoging.WebApi.Infrastructure.Extensions;

public static class ConfigurationsExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        // Регистрация конфигураций с интерфейсами
        services.Configure<KafkaConfig>(configuration.GetSection(KafkaConfig.SECTION_NAME));
        services.AddSingleton<IKafkaConfig>(sp => sp.GetRequiredService<IOptions<KafkaConfig>>().Value);
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Section));
        services.Configure<UptraceConfiguration>(configuration.GetSection(UptraceConfiguration.Section));
        services.AddSingleton<IServiceNotificationlogConsumer, ServiceNotificationlogConsumer>();
        return services;
    }
}