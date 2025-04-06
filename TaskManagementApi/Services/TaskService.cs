using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<TaskItem>> GetPagedAsync(int page, int pageSize)
    {
        _logger.LogInformation("Fetching all tasks");
        var result = await _taskRepository.GetPagedAsync(page, pageSize);
        _logger.LogInformation("Retrieved {Count} tasks", result.Count());
        return result;
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching task with ID {Id}", id);
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            _logger.LogWarning("Task with ID {Id} not found", id);
        }
        return task;
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        _logger.LogInformation("Creating new task: {Title}", task.Title);
        var created = await _taskRepository.CreateAsync(task);
        _logger.LogInformation("Created task with ID {Id}", created.Id);
        return created;
    }

    public async Task<bool> UpdateAsync(TaskItem task)
    {
        _logger.LogInformation("Updating task with ID {Id}", task.Id);
        var updated = await _taskRepository.UpdateAsync(task);
        if (!updated)
        {
            _logger.LogWarning("Update failed for task ID {Id}", task.Id);
        }
        return updated;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting task with ID {Id}", id);
        var deleted = await _taskRepository.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Delete failed for task ID {Id}", id);
        }
        return deleted;
    }
}
