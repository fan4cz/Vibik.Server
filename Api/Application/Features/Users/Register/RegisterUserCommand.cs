using MediatR;
using Shared.Models;

namespace Api.Application.Features.Users.Register;

public record RegisterUserCommand(
    string Username,
    string? DisplayName,
    string Password
) : IRequest<RegisterUserResponse>;