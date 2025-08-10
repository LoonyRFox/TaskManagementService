using TaskManagementService.Application.Interfaces;

namespace TaskManagementService.Persistence.Context;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
    public bool SaveChanges()
    {
        return dbContext.SaveChanges() > 0;
    }
}