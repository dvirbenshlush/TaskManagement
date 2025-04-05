namespace TaskManagementApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string Status { get; set; } = "todo";

    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}
