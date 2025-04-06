using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TaskManagementApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskStatus
{
    [EnumMember(Value = "todo")]
    Todo,

    [EnumMember(Value = "in-progress")]
    InProgress,

    [EnumMember(Value = "done")]
    Done
}
