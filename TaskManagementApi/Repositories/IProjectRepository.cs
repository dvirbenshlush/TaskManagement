using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetPagedAsync(int page, int pageSize);
    Task<Project?> GetByIdAsync(int id);
    Task<Project> CreateAsync(Project project);
    Task<bool> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
}
