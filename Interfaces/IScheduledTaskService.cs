using TaskSchedulerProject.Models;

namespace TaskSchedulerProject.Interfaces
{
    public interface IScheduledTaskService
    {
        Task<IEnumerable<ScheduledTask>> GetAllAsync();
        Task<ScheduledTask?> GetByIdAsync(Guid id);
        Task<(bool success, string? error)> CreateAsync(ScheduledTask task);
        Task<(bool success, string? error)> UpdateAsync(Guid id, ScheduledTask updated);
        Task<(bool success, string? error)> DeleteAsync(Guid id);
    }
}
