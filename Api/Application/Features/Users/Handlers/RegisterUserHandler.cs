using Api.Application.Common.Exceptions;
using Api.Application.Common.Results;
using Api.Application.Features.Users.Commands;
using Api.Application.Features.Users.Validation;
using Infrastructure.Interfaces;
using MediatR;

namespace Api.Application.Features.Users.Handlers;

public class RegisterUserHandler(IUserTable users, IPasswordHasher hasher)
    : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult>>
{
    public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        RegisterUserValidation.Validate(command).EnsureSuccess();

        var username = command.Username;

        var hash = hasher.Hash(command.Password);
        
        var createdUser = await users.RegisterUser(username, hash) ??
                          throw new ApiException("unknown_error", "User creation failed");

        var displayName = string.IsNullOrWhiteSpace(command.DisplayName)
            ? null
            : command.DisplayName!.Trim();

        if (!string.IsNullOrWhiteSpace(displayName) && displayName != createdUser.DisplayName)
        {
            await users.ChangeDisplayName(createdUser.Username, displayName);
            createdUser.DisplayName = displayName;
        }

        var registeredResult = new RegisterUserResult(createdUser, 1);
        return Result<RegisterUserResult>.Ok(registeredResult);
    }
}