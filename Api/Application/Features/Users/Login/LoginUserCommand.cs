using MediatR;
using Shared.Models;

namespace Api.Application.Features.Users.Login;

public record LoginUserCommand(
    string Username,
    string Password
) : IRequest<LoginUserResponse>;