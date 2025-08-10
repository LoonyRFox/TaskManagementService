using System.Xml.Linq;
using TaskManagementService.Application.DTOs;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Application.Interfaces.Repositories;
using TaskManagementService.Domain.Entity;
using TaskManagementService.Persistence.Context;

namespace TaskManagementService.Persistence.Repositories;

public class TaskRepository(ApplicationDbContext dbContext) : GenericRepository<TaskItem>(dbContext), ITaskRepository
{
    public async Task<PaginationResponseDto<TaskDto>> GetPagedListAsync(int pageNumber, int pageSize, string? title)
    {
        var query = dbContext.Tasks.OrderBy(p => p.CreatedAt).AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(p => p.Title.Contains(title));
        }

        return await Paged(
            query.Select(p => new TaskDto(p)),
            pageNumber,
            pageSize);
    }
}