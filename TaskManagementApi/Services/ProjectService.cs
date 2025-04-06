using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using Microsoft.Extensions.Logging;

namespace TaskManagementApi.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(IProjectRepository projectRepository, ILogger<ProjectService> logger)
    {
        _projectRepository = projectRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all projects");
        var result = await _projectRepository.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} projects", result.Count());
        return result;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching project with ID {Id}", id);
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            _logger.LogWarning("Project with ID {Id} not found", id);
        }
        return project;
    }

    public async Task<Project> CreateAsync(Project project)
    {
        _logger.LogInformation("Creating new project: {Name}", project.Name);
        var created = await _projectRepository.CreateAsync(project);
        _logger.LogInformation("Created project with ID {Id}", created.Id);
        return created;
    }

    public async Task<bool> UpdateAsync(Project project)
    {
        _logger.LogInformation("Updating project with ID {Id}", project.Id);
        var updated = await _projectRepository.UpdateAsync(project);
        if (!updated)
        {
            _logger.LogWarning("Update failed for project ID {Id}", project.Id);
        }
        return updated;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting project with ID {Id}", id);
        var deleted = await _projectRepository.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Delete failed for project ID {Id}", id);
        }
        return deleted;
    }
}
