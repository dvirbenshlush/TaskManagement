using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using TaskManagementApi.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly Mock<ILogger<TaskService>> _loggerMock;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _loggerMock = new Mock<ILogger<TaskService>>();
        _taskService = new TaskService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTask_WhenExists()
    {
        var task = new TaskItem { Id = 1, Title = "Test Task" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

        var result = await _taskService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test Task", result?.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItem?)null);

        var result = await _taskService.GetByIdAsync(123);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CreatesTaskSuccessfully()
    {
        var task = new TaskItem { Title = "New Task" };
        var created = new TaskItem { Id = 1, Title = "New Task" };

        _repositoryMock.Setup(r => r.CreateAsync(task)).ReturnsAsync(created);

        var result = await _taskService.CreateAsync(task);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Task", result.Title);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsTrue_WhenSuccess()
    {
        var task = new TaskItem { Id = 1, Title = "Updated" };

        _repositoryMock.Setup(r => r.UpdateAsync(task)).ReturnsAsync(true);

        var result = await _taskService.UpdateAsync(task);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenFail()
    {
        var task = new TaskItem { Id = 1, Title = "Updated" };

        _repositoryMock.Setup(r => r.UpdateAsync(task)).ReturnsAsync(false);

        var result = await _taskService.UpdateAsync(task);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleted()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _taskService.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(123)).ReturnsAsync(false);

        var result = await _taskService.DeleteAsync(123);

        Assert.False(result);
    }
}
