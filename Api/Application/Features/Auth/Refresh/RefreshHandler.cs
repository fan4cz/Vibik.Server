using Infrastructure.Interfaces;
using MediatR;
using Shared.Models;

namespace Api.Application.Features.Auth.Refresh;

public class RefreshHandler(IUserTable users, IPasswordHasher hasher, ITokenService tokenService)
    : IRequestHandler<RefreshCommand, RefreshResponse>
{
    public async Task<RefreshResponse> Handle(RefreshCommand command,
        CancellationToken cancellationToken)
    {
        var username = command.Username;
        return new RefreshResponse(
            tokenService.GenerateAccessToken(username),
            tokenService.GenerateRefreshToken(username)
        );
    }
}