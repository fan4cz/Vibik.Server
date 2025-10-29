namespace Api.Application.Features.Users.Models;

public sealed class RegisterRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string? DisplayName { get; init; } = null!;
}