using Api.Application.Common.Exceptions;
using Api.Application.Features.Users.Login;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.VisualBasic.CompilerServices;
using Shared.Models;

namespace Api.Application.Features.Users.Login;

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