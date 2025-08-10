using TaskManagementService.Domain.Entity;

namespace TaskManagementService.Application.DTOs.Task;

/// <summary>DTO задачи для выдачи через API.</summary>
public class TaskDto
{
#pragma warning disable
    public TaskDto()
    {

    }
#pragma warning restore
    public TaskDto(TaskItem item)
    {
        Id = item.Id;
        Title = item.Title;
        Description = item.Description;
        Status = item.Status;
        CreatedDateTime = item.CreatedAt;
        UpdateDateTime = item.UpdatedAt;
        CreateBy = item.CreatedBy;
       
    }

    public long Id { get; set; }
    public string Title { get;  set; }
    public string Description { get;  set; }
    public string Status { get;  set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? UpdateDateTime { get; set; }
    public Guid? CreateBy { get; set; }
   
}
