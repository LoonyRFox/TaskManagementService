using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using TaskManagementService.Application.Interfaces.Messaging;
using TaskManagementService.Messaging.Interfaces;
using TaskManagementService.Messaging.KafkaMessaging;
using TaskManagementService.Messaging.Services;

namespace TaskManagementService.Messaging
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddMessagingInfrastructure(this IServiceCollection services)
        {
            // Kafka producer (может использоваться при выборе DeliveryMode = "Kafka")
            services.AddSingleton<IMessageProducer, KafkaProducer>();

            // Настройки Refit
            var refitSettings = new RefitSettings
            {
                CollectionFormat = CollectionFormat.Multi
            };

            // Регистрируем Refit клиент для HTTP доставки
            services.AddRefitClient<ITaskManagementLogHttpClient>(refitSettings)
                .ConfigureHttpClient((sp, client) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();
                    var baseUrl = config.GetValue<string>("Outbox:Http:BaseUrl");

                    if (string.IsNullOrWhiteSpace(baseUrl))
                        throw new InvalidOperationException("Missing Outbox:Http:BaseUrl in configuration.");

                    client.BaseAddress = new Uri(baseUrl);
                });

            // Регистрируем IEventPublisher с выбором режима доставки
            services.AddSingleton<IEventPublisher>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var deliveryMode = config["Outbox:DeliveryMode"];

                return deliveryMode switch
                {
                    "Kafka" => ActivatorUtilities.CreateInstance<NotificationServiceProducer>(sp),
                    "Http" => ActivatorUtilities.CreateInstance<HttpEventSender>(sp),
                    _ => throw new InvalidOperationException($"Unknown DeliveryMode: {deliveryMode}")
                };
            });

            // Outbox Processor
            services.AddScoped<OutboxProcessor>();

            // Фоновая служба обработки сообщений
            services.AddHostedService<OutboxBackgroundService>();

            return services;
        }
    }
}
