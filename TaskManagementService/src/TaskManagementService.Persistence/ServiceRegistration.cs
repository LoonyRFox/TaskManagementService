using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.Interfaces.Repositories;
using TaskManagementService.Persistence.Configurations;
using TaskManagementService.Persistence.Context;
using TaskManagementService.Persistence.Repositories;

namespace TaskManagementService.Persistence
{
    public static class ServiceRegistration

    {
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration, bool useInMemoryDatabase)
        {
            if (useInMemoryDatabase)
            {
                // Подключение InMemory DB (для unit-тестов или демо)
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(nameof(ApplicationDbContext)));
            }
            else
            {
                // Читаем настройки подключения к БД из конфигурации
                var dbOptions = new DatabaseOptions();
                configuration.GetSection(DatabaseOptions.Section).Bind(dbOptions);

                if (string.IsNullOrEmpty(dbOptions.Name))
                {
                    throw new InvalidOperationException("Database configuration is missing");
                }

                // Формируем строку подключения к PostgreSQL
                var connectionString = new NpgsqlConnectionStringBuilder
                {
                    Host = dbOptions.Host,
                    Port = dbOptions.Port,
                    Database = dbOptions.Name,
                    Username = dbOptions.User,
                    Password = dbOptions.Password,
                    Pooling = dbOptions.Pooling,
                    KeepAlive = dbOptions.KeepAlive,
                    TcpKeepAlive = dbOptions.TcpKeepAlive
                }.ToString();

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(connectionString, npgsqlOptions =>
                    {
                        npgsqlOptions.CommandTimeout(dbOptions.CommandTimeout);
                        npgsqlOptions.EnableRetryOnFailure(); // Ретраи при временных ошибках сети
                    });

                    // Если хотим логировать SQL-запросы в консоль
                    if (dbOptions.ShowSqlQuery)
                    {
                        options.LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging();
                    }
                });
            }

            // Регистрируем UnitOfWork и репозитории
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.RegisterRepositories();

            return services;
        }
        private static void RegisterRepositories(this IServiceCollection services)
        {
            var interfaceType = typeof(IGenericRepository<>);
            var interfaces = Assembly.GetAssembly(interfaceType)!.GetTypes()
                .Where(p => p.GetInterface(interfaceType.Name) != null);

            var implementations = Assembly.GetAssembly(typeof(GenericRepository<>))!.GetTypes();

            foreach (var item in interfaces)
            {
                var implementation = implementations.FirstOrDefault(p => p.GetInterface(item.Name) != null);

                if (implementation is not null)
                    services.AddTransient(item, implementation);
            }
        }
    }
}
