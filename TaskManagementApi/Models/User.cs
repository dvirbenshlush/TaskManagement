using System.Text.Json.Serialization;

namespace TaskManagementApi.Models;

public class User
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = "user";
}