using MediatR;
using Shared.Models;

namespace Api.Application.Features.Auth.Register;

public record RegisterUserCommand(
    string Username,
    string? DisplayName,
    string Password
) : IRequest<RegisterUserResponse>;