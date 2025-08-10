namespace TaskManagementService.Domain.Entity.Common;

/// <summary>
/// Абстрактная сущность, содержащая поля аудита — автора, дату создания и обновления.
/// Параметризуется типом ключа <typeparamref name="TKey"/>.
/// </summary>
/// <typeparam name="TKey">Тип уникального идентификатора сущности.</typeparam>
public abstract class AuditableBaseEntity<TKey> : BaseEntity<TKey>
{
    /// <summary>
    /// Идентификатор пользователя (агента), создавшего запись.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Дата и время создания записи (в формате UTC).
    /// Значение по умолчанию — текущий момент.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата и время последнего обновления записи (UTC).
    /// Может быть <c>null</c>, если обновлений ещё не было.
    /// </summary>
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
/// <summary>
/// Версия <see cref="AuditableBaseEntity{TKey}"/> с типом ключа <see cref="long"/>.
/// </summary>
public abstract class AuditableBaseEntity : AuditableBaseEntity<long>
{
}