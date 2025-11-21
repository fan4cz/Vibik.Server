using Api.Application.Common.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models;

namespace Api.Application.Features.Auth.Login;

public class LoginUserHandler(IUserTable users, IPasswordHasher hasher, ITokenService tokenService)
    : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserCommand command,
        CancellationToken cancellationToken)
    {
        LoginUserValidation.Validate(command);

        var username = command.Username;

        var hash = hasher.Hash(command.Password);
        var loginStatus = await users.CheckPassword(username, hash);

        if (loginStatus)
            return new LoginUserResponse(tokenService.GenerateAccessToken(username),
                tokenService.GenerateRefreshToken(username));
        throw new ApiException(StatusCodes.Status401Unauthorized, "Incorrect password or username");
    }
}