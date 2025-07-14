using Microsoft.EntityFrameworkCore;
using TaskSchedulerProject.Models;

namespace TaskSchedulerProject.DataAccess.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<ScheduledTask> ScheduledTasks => Set<ScheduledTask>();
    }
}
