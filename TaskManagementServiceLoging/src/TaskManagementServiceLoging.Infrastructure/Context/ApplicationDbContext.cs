using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using TaskManagementServiceLoging.Domain.Entity;

namespace TaskManagementServiceLoging.Infrastructure.Context;

public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<LogModel> LogModels { get; set; }
  
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}