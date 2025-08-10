namespace TaskManagementService.Domain.Entity.Common;

/// <summary>
/// Базовый класс для всех сущностей, содержащий идентификатор и флаг активности.
/// </summary>
/// <typeparam name="TKey">Тип уникального идентификатора.</typeparam>
public abstract class BaseEntity<TKey>
{
    /// <summary>
    /// Уникальный идентификатор сущности.
    /// </summary>
    public TKey Id { get; set; } = default!;

    /// <summary>
    /// Флаг активности сущности. <c>true</c> — активна, <c>false</c> — деактивирована.
    /// </summary>
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Версия <see cref="BaseEntity{TKey}"/> с типом ключа <see cref="long"/>.
/// </summary>
public abstract class BaseEntity : BaseEntity<long>
{
}