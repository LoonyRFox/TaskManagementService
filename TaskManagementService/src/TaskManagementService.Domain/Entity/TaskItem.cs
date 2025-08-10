using TaskManagementService.Domain.Entity.Common;

namespace TaskManagementService.Domain.Entity;
/// <summary>
/// Представляет задачу в системе управления задачами.
/// Содержит заголовок, описание и статус.
/// Наследуется от <see cref="AuditableBaseEntity"/>, чтобы включать данные об авторе и датах изменений.
/// </summary>
public class TaskItem : AuditableBaseEntity
{
    /// <summary>
    /// Заголовок задачи.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Статус задачи (например, "Новая", "В работе", "Завершена").
    /// </summary>
    public string Status { get; private set; }

#pragma warning disable
    // Приватный конструктор нужен для EF Core, чтобы он мог
    // создавать объект без вызова публичных конструкторов.
    private TaskItem()
    {
    }
#pragma warning disable

    /// <summary>
    /// Создаёт новый экземпляр задачи.
    /// </summary>
    /// <param name="title">Заголовок задачи.</param>
    /// <param name="description">Описание задачи.</param>
    /// <param name="status">Статус задачи.</param>
    public TaskItem(string title, string description, string status)
    {
        Title = title;
        Description = description;
        Status = status;
    }

    /// <summary>
    /// Обновляет данные задачи.
    /// </summary>
    /// <param name="title">Новый заголовок.</param>
    /// <param name="description">Новое описание.</param>
    /// <param name="status">Новый статус.</param>
    public void Update(string title, string description, string status)
    {
        Title = title;
        Description = description;
        Status = status;
    }
}
