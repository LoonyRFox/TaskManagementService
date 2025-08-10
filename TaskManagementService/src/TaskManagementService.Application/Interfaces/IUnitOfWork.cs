namespace TaskManagementService.Application.Interfaces;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync();
}