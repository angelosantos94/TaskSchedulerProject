using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskSchedulerProject.DataAccess.Context;
using TaskSchedulerProject.Models;

namespace DeferredTaskAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduledTasksController : ControllerBase
{
    private readonly AppDBContext _context;

    public ScheduledTasksController(AppDBContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.ScheduledTasks.AsNoTracking().ToListAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _context.ScheduledTasks.FindAsync(id);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ScheduledTask task)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (task.ScheduledTime <= DateTime.UtcNow)
            return BadRequest("ScheduledTime must be in the future.");

        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;
        task.IsExecuted = false;

        _context.ScheduledTasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ScheduledTask updated)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (updated.ScheduledTime <= DateTime.UtcNow)
            return BadRequest("ScheduledTime must be in the future.");

        var task = await _context.ScheduledTasks.FindAsync(id);
        if (task == null) return NotFound();
        if (task.IsExecuted) return BadRequest("Executed tasks cannot be updated.");

        task.Title = updated.Title;
        task.Description = updated.Description;
        task.ScheduledTime = updated.ScheduledTime;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var task = await _context.ScheduledTasks.FindAsync(id);
        if (task == null) return NotFound();
        if (task.IsExecuted) return BadRequest("Executed tasks cannot be deleted.");

        _context.ScheduledTasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
