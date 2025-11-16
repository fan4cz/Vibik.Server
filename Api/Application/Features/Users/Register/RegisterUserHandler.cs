using Api.Application.Common.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models;

namespace Api.Application.Features.Users.Register;

public class RegisterUserHandler(IUserTable users, IPasswordHasher hasher)
    : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        RegisterUserValidation.Validate(command);

        var username = command.Username;

        var hash = hasher.Hash(command.Password);

        var createdUser = await users.RegisterUser(username, hash) ??
                          throw new ApiException(StatusCodes.Status500InternalServerError, "User creation failed");

        var displayName = string.IsNullOrWhiteSpace(command.DisplayName)
            ? null
            : command.DisplayName.Trim();

        if (displayName is not null && displayName != createdUser.DisplayName)
        {
            await users.ChangeDisplayName(createdUser.Username, displayName);
            createdUser.DisplayName = displayName;
        }

        var registeredResult = new RegisterUserResponse(true);
        return registeredResult;
    }
}