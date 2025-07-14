using TaskSchedulerProject.Models;

namespace TaskSchedulerProject.Interfaces
{
    public interface IScheduledTaskRepository
    {
        Task<IEnumerable<ScheduledTask>> GetAllAsync();
        Task<ScheduledTask?> GetByIdAsync(Guid id);
        Task AddAsync(ScheduledTask task);
        Task UpdateAsync(ScheduledTask task);
        Task DeleteAsync(ScheduledTask task);
        Task SaveChangesAsync();
    }
}
