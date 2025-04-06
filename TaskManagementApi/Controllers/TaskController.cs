using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManagementApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TaskController> _logger;

    public TaskController(ITaskService taskService, ILogger<TaskController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetPagedAsync(int page = 1, int pageSize = 3)
    {
        _logger.LogInformation("Getting all tasks");
        var tasks = await _taskService.GetPagedAsync(page, pageSize);
        _logger.LogInformation("Retrieved {Count} tasks", tasks.Count());
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        _logger.LogInformation("Getting task with ID {Id}", id);
        var task = await _taskService.GetByIdAsync(id);
        if (task == null)
        {
            _logger.LogWarning("Task with ID {Id} not found", id);
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(TaskItem task)
    {
        _logger.LogInformation("Creating task with title: {Title}", task.Title);
        var created = await _taskService.CreateAsync(task);
        _logger.LogInformation("Task created with ID {Id}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskItem task)
    {
        if (id != task.Id)
        {
            _logger.LogWarning("Task ID in URL ({UrlId}) does not match Task body ID ({BodyId})", id, task.Id);
            return BadRequest();
        }

        _logger.LogInformation("Updating task with ID {Id}", id);
        var updated = await _taskService.UpdateAsync(task);
        if (!updated)
        {
            _logger.LogWarning("Task update failed – task ID {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Task with ID {Id} updated successfully", id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting task with ID {Id}", id);
        var deleted = await _taskService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Task delete failed – task ID {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Task with ID {Id} deleted successfully", id);
        return NoContent();
    }
}
