using Api.Application.Common.Results;
using Infrastructure.Interfaces;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Commands.Users.RegisterUser;

// TODO: валидацию вынести можно
public class RegisterUserHandler(IUserTable users, IPasswordHasher hasher)
    : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult>>
{
    public Task<Result<RegisterUserResult>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var username = command.Username.Trim();
        if (string.IsNullOrWhiteSpace(username))
            return Task.FromResult(Result<RegisterUserResult>.Fail("validation", "Username is required"));

        if (string.IsNullOrEmpty(command.Password) || command.Password.Length < 5)
            return Task.FromResult(Result<RegisterUserResult>.Fail("validation", "Password is too short"));

        var hash = hasher.Hash(command.Password);

        var createdUser = users.RegisterUser(username, hash);
        if (createdUser is null)
            return Task.FromResult(Result<RegisterUserResult>.Fail("username_taken", "Username is taken"));
        
        var displayName = string.IsNullOrWhiteSpace(command.DisplayName) ? null : command.DisplayName!.Trim();

        if (!string.IsNullOrWhiteSpace(displayName) && displayName != createdUser.DisplayName)
        {
            users.ChangeDisplayName(createdUser.Username, displayName);
            createdUser.DisplayName = displayName;
        }
        
        var registeredResult = new RegisterUserResult(createdUser, 1);
        return Task.FromResult(Result<RegisterUserResult>.Ok(registeredResult));
    }
}