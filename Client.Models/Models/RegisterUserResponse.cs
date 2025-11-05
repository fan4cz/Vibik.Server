namespace Shared.Models;

public record RegisterUserResponse(
    User User,
    int SessionId
);