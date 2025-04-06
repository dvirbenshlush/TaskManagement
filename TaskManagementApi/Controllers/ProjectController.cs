using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace TaskManagementApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetAll()
    {
        _logger.LogInformation("Getting all projects");
        var projects = await _projectService.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} projects", projects.Count());
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetById(int id)
    {
        _logger.LogInformation("Getting project with ID {Id}", id);
        var project = await _projectService.GetByIdAsync(id);
        if (project == null)
        {
            _logger.LogWarning("Project with ID {Id} not found", id);
            return NotFound();
        }

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<Project>> Create(Project project)
    {
        _logger.LogInformation("Creating new project: {Name}", project.Name);
        var created = await _projectService.CreateAsync(project);
        _logger.LogInformation("Project created with ID {Id}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Project project)
    {
        if (id != project.Id)
        {
            _logger.LogWarning("Update failed: ID mismatch. URL ID: {UrlId}, Body ID: {BodyId}", id, project.Id);
            return BadRequest();
        }

        _logger.LogInformation("Updating project with ID {Id}", id);
        var updated = await _projectService.UpdateAsync(project);
        if (!updated)
        {
            _logger.LogWarning("Update failed: Project with ID {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Project with ID {Id} updated successfully", id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting project with ID {Id}", id);
        var deleted = await _projectService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Delete failed: Project with ID {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Project with ID {Id} deleted successfully", id);
        return NoContent();
    }
}
