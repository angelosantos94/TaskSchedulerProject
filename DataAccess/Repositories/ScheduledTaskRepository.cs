using Microsoft.EntityFrameworkCore;
using TaskSchedulerProject.DataAccess.Context;
using TaskSchedulerProject.Interfaces;
using TaskSchedulerProject.Models;

namespace DeferredTaskAPI.Infrastructure.Repositories;

public class ScheduledTaskRepository : IScheduledTaskRepository
{
    private readonly AppDBContext _context;
    public ScheduledTaskRepository(AppDBContext context) => _context = context;

    public async Task<IEnumerable<ScheduledTask>> GetAllAsync() =>
        await _context.ScheduledTasks.AsNoTracking().ToListAsync();

    public async Task<ScheduledTask?> GetByIdAsync(Guid id) =>
        await _context.ScheduledTasks.FindAsync(id);

    public async Task AddAsync(ScheduledTask task) =>
        await _context.ScheduledTasks.AddAsync(task);

    public Task UpdateAsync(ScheduledTask task)
    {
        _context.ScheduledTasks.Update(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ScheduledTask task)
    {
        _context.ScheduledTasks.Remove(task);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
