using Microsoft.EntityFrameworkCore;
using TaskManagementService.Domain.Entity;
using TaskManagementService.Domain.Enums;
using TaskManagementService.Persistence.Context;

namespace TaskManagementService.Persistence.Seed;

public static class TaskDefaultData
{
    public static async Task SeedAsync(ApplicationDbContext applicationDbContext)
    {
        if (!await applicationDbContext.Tasks.AnyAsync())
        {
            List<TaskItem> defaultProducts = [
                new TaskItem("Task 1","Очень информативное описание задачи номер 1",nameof(CustomTaskStatus.New)),
                new TaskItem("Task 2","Очень информативное описание задачи номер 2",nameof(CustomTaskStatus.New)),
                new TaskItem("Task 3","Очень информативное описание задачи номер 3",nameof(CustomTaskStatus.New)),
                new TaskItem("Task 4","Очень информативное описание задачи номер 4",nameof(CustomTaskStatus.New)),
                new TaskItem("Task 5","Очень информативное описание задачи номер 5",nameof(CustomTaskStatus.New)),
                new TaskItem("Task 6","Очень информативное описание задачи номер 6",nameof(CustomTaskStatus.New))
            ];

            await applicationDbContext.Tasks.AddRangeAsync(defaultProducts);

            await applicationDbContext.SaveChangesAsync();
        }
    }
}