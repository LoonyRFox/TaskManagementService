using TaskManagementServiceLoging.Application.DTOs;
using TaskManagementServiceLoging.Application.DTOs.Log;
using TaskManagementServiceLoging.Domain.Entity;

namespace TaskManagementServiceLoging.Application.Interfaces.Repositories;

public interface ILogRepository : IGenericRepository<LogModel>
{
    Task<PaginationResponseDto<LogDto>> GetPagedListAsync(int pageNumber, int pageSize, string? type);
}