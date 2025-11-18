using MediatR;
using Shared.Models;

namespace Api.Application.Features.Users.Login;

public record RefreshCommand(
    string Username
    ) : IRequest<RefreshResponse>;
