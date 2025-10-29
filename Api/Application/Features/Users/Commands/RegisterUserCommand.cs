using Api.Application.Common.Results;
using MediatR;
using Shared.Models;

namespace Api.Application.Features.Users.Commands;

public record RegisterUserCommand(
    string Username,
    string? DisplayName,
    string Password
) : IRequest<Result<RegisterUserResult>>;

public record RegisterUserResult(
    User User,
    int SessionId
);