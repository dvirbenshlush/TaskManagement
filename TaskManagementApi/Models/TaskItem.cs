using System.Text.Json.Serialization;

namespace TaskManagementApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    public int ProjectId { get; set; }
    [JsonIgnore]
    public Project? Project { get; set; }
}
