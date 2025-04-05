using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _projectRepository.GetAllAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _projectRepository.GetByIdAsync(id);
    }

    public async Task<Project> CreateAsync(Project project)
    {
        return await _projectRepository.CreateAsync(project);
    }

    public async Task<bool> UpdateAsync(Project project)
    {
        return await _projectRepository.UpdateAsync(project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _projectRepository.DeleteAsync(id);
    }
}

