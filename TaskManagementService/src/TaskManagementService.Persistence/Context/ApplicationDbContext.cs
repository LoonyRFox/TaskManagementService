using Microsoft.EntityFrameworkCore;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Domain.Entity;
using TaskManagementService.Domain.Messages;
using TaskManagementService.Persistence.Extensions;


namespace TaskManagementService.Persistence.Context;

public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options, IAuthenticatedUserService authenticatedUser) : DbContext(options)
{
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ChangeTracker.ApplyAuditing(authenticatedUser);

        this.AddDomainEventsToOutbox();

        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        this.ConfigureDecimalProperties(builder);

        base.OnModelCreating(builder);
    }
}