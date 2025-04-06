namespace TaskManagementApi.Settings;

public class CloudWatchLoggingSettings
{
    public string LogGroup { get; set; } = "TaskManagementLogs";
    public string Region { get; set; } = "us-east-1";
    public string AccessKey { get; set; } = ""; 
    public string SecretKey { get; set; } = "";
}
