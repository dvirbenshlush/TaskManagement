using TaskManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementApi.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProjectRepository> _logger;

    public ProjectRepository(AppDbContext context, ILogger<ProjectRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Project>> GetPagedAsync(int page, int pageSize)
    {
        try
        {
            _logger.LogInformation("Fetching all projects from database");
            var result = await _context.Projects
            .Include(p => p.Tasks)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            _logger.LogInformation("Fetched {Count} projects", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching projects");
            throw;
        }
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching project with ID {Id}", id);
        var project = await _context.Projects.Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (project == null)
            _logger.LogWarning("Project with ID {Id} not found", id);
        return project;
    }

    public async Task<Project> CreateAsync(Project project)
    {
        _logger.LogInformation("Creating new project: {Name}", project.Name);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created project with ID {Id}", project.Id);
        return project;
    }

    public async Task<bool> UpdateAsync(Project project)
    {
        _logger.LogInformation("Attempting to update project with ID {Id}", project.Id);

        var exists = await _context.Projects.AnyAsync(p => p.Id == project.Id);
        if (!exists)
        {
            _logger.LogWarning("Update failed: Project ID {Id} not found", project.Id);
            return false;
        }

        _context.Entry(project).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Project with ID {Id} updated successfully", project.Id);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Attempting to delete project with ID {Id}", id);

        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            _logger.LogWarning("Delete failed: Project ID {Id} not found", id);
            return false;
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Project with ID {Id} deleted successfully", id);
        return true;
    }
}
