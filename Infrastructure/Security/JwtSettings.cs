namespace Infrastructure.Security;

public class JwtSettings
{
    public TimeSpan Expires { get; init; }
    public string SecretKey { get; init; } = string.Empty;
}