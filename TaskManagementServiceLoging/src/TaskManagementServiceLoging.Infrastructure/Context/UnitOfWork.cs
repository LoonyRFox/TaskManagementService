using Microsoft.EntityFrameworkCore;
using TaskManagementServiceLoging.Application.Interfaces;

namespace TaskManagementServiceLoging.Infrastructure.Context;

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