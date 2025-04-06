using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using TaskManagementApi.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _repositoryMock;
    private readonly Mock<ILogger<ProjectService>> _loggerMock;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _loggerMock = new Mock<ILogger<ProjectService>>();
        _projectService = new ProjectService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProject_WhenExists()
    {
        // Arrange
        var project = new Project { Id = 1, Name = "Test Project" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);

        // Act
        var result = await _projectService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Project", result?.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project?)null);

        var result = await _projectService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CreatesProjectSuccessfully()
    {
        var project = new Project { Name = "New Project" };
        var created = new Project { Id = 1, Name = "New Project" };

        _repositoryMock.Setup(r => r.CreateAsync(project)).ReturnsAsync(created);

        var result = await _projectService.CreateAsync(project);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Project", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsTrue_WhenSuccess()
    {
        var project = new Project { Id = 1, Name = "Updated" };

        _repositoryMock.Setup(r => r.UpdateAsync(project)).ReturnsAsync(true);

        var result = await _projectService.UpdateAsync(project);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenFail()
    {
        var project = new Project { Id = 1, Name = "Updated" };

        _repositoryMock.Setup(r => r.UpdateAsync(project)).ReturnsAsync(false);

        var result = await _projectService.UpdateAsync(project);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleted()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _projectService.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _projectService.DeleteAsync(99);

        Assert.False(result);
    }
}
