using System.ComponentModel.DataAnnotations;

namespace TaskSchedulerProject.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ScheduledTask
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }

        public bool IsExecuted { get; set; }

        public DateTime? ExecutedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
