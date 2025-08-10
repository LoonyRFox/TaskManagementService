using TaskManagementService.Application.DTOs;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Domain.Entity;

namespace TaskManagementService.Application.Interfaces.Repositories;

public interface ITaskRepository : IGenericRepository<TaskItem>
{
    Task<PaginationResponseDto<TaskDto>> GetPagedListAsync(int pageNumber, int pageSize, string? title);
}