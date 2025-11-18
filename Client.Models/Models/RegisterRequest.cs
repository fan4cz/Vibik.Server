namespace Shared.Models;

public sealed class RegisterRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string? DisplayName { get; init; } = null!;
}