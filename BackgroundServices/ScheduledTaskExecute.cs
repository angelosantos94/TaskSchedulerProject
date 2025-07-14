using Microsoft.EntityFrameworkCore;
using TaskSchedulerProject.DataAccess.Context;

namespace TaskSchedulerProject.BackgroundServices;

public class ScheduledTaskExecute : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduledTaskExecute> _logger;

    public ScheduledTaskExecute(IServiceProvider serviceProvider, ILogger<ScheduledTaskExecute> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            var now = DateTime.UtcNow;

            var dueTasks = await context.ScheduledTasks
                .Where(t => !t.IsExecuted && t.ScheduledTime <= now)
                .ToListAsync(stoppingToken);

            foreach (var task in dueTasks)
            {
                task.IsExecuted = true;
                task.ExecutedAt = now;
                task.UpdatedAt = now;

                _logger.LogInformation($"Executed Task: {task.Title} at {now}");
            }

            await context.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
