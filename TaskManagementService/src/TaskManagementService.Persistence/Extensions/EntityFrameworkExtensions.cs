using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Domain.Entity.Common;

namespace TaskManagementService.Persistence.Extensions;

public static class EntityFrameworkExtensions
{
    /// <summary>
    /// Применяет информацию об аудите (кто и когда создал/изменил запись) 
    /// ко всем сущностям, которые реализуют <see cref="AuditableBaseEntity"/>.
    /// </summary>
    /// <param name="changeTracker">Экземпляр <see cref="ChangeTracker"/>, отслеживающий изменения в сущностях.</param>
    /// <param name="authenticatedUser">Сервис, предоставляющий информацию о текущем пользователе.</param>
    /// <remarks>
    /// Этот метод обычно вызывается перед сохранением изменений в базе данных (<c>SaveChanges</c> или <c>SaveChangesAsync</c>).
    /// Если пользователь не аутентифицирован, в качестве идентификатора пользователя будет использован <see cref="Guid.Empty"/>.
    /// </remarks>
    public static void ApplyAuditing(this ChangeTracker changeTracker, IAuthenticatedUserService authenticatedUser)
    {
        // Получаем идентификатор текущего пользователя или Guid.Empty, если пользователь не аутентифицирован
        var userId = string.IsNullOrEmpty(authenticatedUser.UserId)
            ? Guid.Empty
            : Guid.Parse(authenticatedUser.UserId);

        // Запоминаем текущее UTC время для записи в поля аудита
        var currentTime = DateTime.UtcNow;

        // Проходим по всем отслеживаемым сущностям
        foreach (var entry in changeTracker.Entries())
        {
            var entityType = entry.Entity.GetType();

            // Проверяем, является ли сущность аудируемой
            if (typeof(AuditableBaseEntity).IsAssignableFrom(entityType) ||
                (entityType.BaseType?.IsGenericType ?? false) &&
                entityType.BaseType.GetGenericTypeDefinition() == typeof(AuditableBaseEntity<>))
            {
                // Динамически приводим объект к типу с полями Created, CreatedBy, LastModified и LastModifiedBy
                dynamic auditableEntity = entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    // Если это новая запись — устанавливаем дату и пользователя создания
                    auditableEntity.CreatedAt = currentTime;
                    auditableEntity.CreatedBy = userId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Если запись изменена — обновляем дату и пользователя последнего изменения
                    auditableEntity.UpdatedAt = currentTime;
                    //auditableEntity.LastModifiedBy = userId;
                }
            }
        }
    }

    /// <summary>
    /// Настраивает свойства типа <see cref="decimal"/> для всех сущностей в <see cref="DbContext"/>,
    /// устанавливая точность (precision) 18 и масштаб (scale) 6.
    /// </summary>
    /// <param name="context">Экземпляр <see cref="DbContext"/>, к которому применяется конфигурация.</param>
    /// <param name="builder">Экземпляр <see cref="ModelBuilder"/> для настройки моделей EF Core.</param>
    /// <remarks>
    /// Этот метод помогает унифицировать хранение числовых значений с плавающей точкой в базе данных.
    /// Также он автоматически применяет все конфигурации сущностей, 
    /// реализованные через <see cref="IEntityTypeConfiguration{TEntity}"/> в текущей сборке.
    /// </remarks>
    public static void ConfigureDecimalProperties(this DbContext context, ModelBuilder builder)
    {
        // Находим все свойства decimal и decimal?, чтобы задать им тип "decimal(18,6)" в БД
        foreach (var property in builder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,6)");
        }

        // Применяем все конфигурации из текущей сборки (например, Fluent API маппинги)
        builder.ApplyConfigurationsFromAssembly(context.GetType().Assembly);
    }
}