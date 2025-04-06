using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManagementApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<TaskRepository> _logger;

    public TaskRepository(AppDbContext context, ILogger<TaskRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TaskItem>> GetPagedAsync(int page, int pageSize)
    {
        _logger.LogInformation("Fetching all tasks from database");
        var result = await _context.Tasks
               .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        _logger.LogInformation("Fetched {Count} tasks", result.Count);
        return result;
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching task with ID {Id}", id);
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            _logger.LogWarning("Task with ID {Id} not found", id);
        }
        return task;
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        _logger.LogInformation("Creating new task: {Title}", task.Title);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created task with ID {Id}", task.Id);
        return task;
    }

    public async Task<bool> UpdateAsync(TaskItem task)
    {
        _logger.LogInformation("Attempting to update task with ID {Id}", task.Id);

        var exists = await _context.Tasks.AnyAsync(t => t.Id == task.Id);
        if (!exists)
        {
            _logger.LogWarning("Update failed: Task ID {Id} not found", task.Id);
            return false;
        }

        _context.Entry(task).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Task with ID {Id} updated successfully", task.Id);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Attempting to delete task with ID {Id}", id);

        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            _logger.LogWarning("Delete failed: Task ID {Id} not found", id);
            return false;
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Task with ID {Id} deleted successfully", id);
        return true;
    }
}
