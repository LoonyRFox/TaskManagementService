using TaskManagementServiceLoging.Application.DTOs;
using TaskManagementServiceLoging.Application.DTOs.Log;
using TaskManagementServiceLoging.Application.Interfaces.Repositories;
using TaskManagementServiceLoging.Domain.Entity;
using TaskManagementServiceLoging.Infrastructure.Context;

namespace TaskManagementServiceLoging.Infrastructure.Repositories;

public class LogRepository(ApplicationDbContext dbContext) : GenericRepository<LogModel>(dbContext), ILogRepository
{
    public async Task<PaginationResponseDto<LogDto>> GetPagedListAsync(int pageNumber, int pageSize, string? type)
    {
        var query = dbContext.LogModels.OrderBy(p => p.ProcessedOnUtc).AsQueryable();

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(p => p.Type.Contains(type));
        }
        return await Paged(
            query.Select(p => new LogDto(p)),
            pageNumber,
            pageSize);
    }
}