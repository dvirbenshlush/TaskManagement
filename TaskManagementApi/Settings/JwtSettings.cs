namespace TaskManagementApi.Settings;

public class JwtSettings
{
    public string Authority { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public bool ValidateAudience { get; set; }
}
