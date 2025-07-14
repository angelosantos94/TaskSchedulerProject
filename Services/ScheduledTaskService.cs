using TaskSchedulerProject.Interfaces;
using TaskSchedulerProject.Models;

namespace DeferredTaskAPI.Application.Services;

public class ScheduledTaskService : IScheduledTaskService
{
    private readonly IScheduledTaskRepository _repository;

    public ScheduledTaskService(IScheduledTaskRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ScheduledTask>> GetAllAsync() => _repository.GetAllAsync();

    public Task<ScheduledTask?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task<(bool success, string? error)> CreateAsync(ScheduledTask task)
    {
        if (task.ScheduledTime <= DateTime.UtcNow)
            return (false, "ScheduledTime must be in the future.");

        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;
        task.IsExecuted = false;

        await _repository.AddAsync(task);
        await _repository.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool success, string? error)> UpdateAsync(Guid id, ScheduledTask updated)
    {
        if (updated.ScheduledTime <= DateTime.UtcNow)
            return (false, "ScheduledTime must be in the future.");

        var task = await _repository.GetByIdAsync(id);
        if (task is null) return (false, "Task not found.");
        if (task.IsExecuted) return (false, "Executed tasks cannot be updated.");

        task.Title = updated.Title;
        task.Description = updated.Description;
        task.ScheduledTime = updated.ScheduledTime;
        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task);
        await _repository.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool success, string? error)> DeleteAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null) return (false, "Task not found.");
        if (task.IsExecuted) return (false, "Executed tasks cannot be deleted.");

        await _repository.DeleteAsync(task);
        await _repository.SaveChangesAsync();
        return (true, null);
    }
}
