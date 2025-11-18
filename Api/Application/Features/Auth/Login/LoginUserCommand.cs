using MediatR;
using Shared.Models;

namespace Api.Application.Features.Auth.Login;

public record LoginUserCommand(
    string Username,
    string Password
) : IRequest<LoginUserResponse>;