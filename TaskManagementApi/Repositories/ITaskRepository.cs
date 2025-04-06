using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetPagedAsync(int page, int pageSize);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<bool> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id);
}