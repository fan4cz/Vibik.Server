using Api.Application.Common.Results;
using Infrastructure.Interfaces;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Commands.Users.RegisterUser;

//Если я правильно понимаю, что хешируем пароль мы на этом этапе
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

        //Насколько я поняла по тому, как у нас сделан RegisterUser, displayName это не обязательный для ввода аргумент,
        //если его нет, то мы по умолчанию делаем displayName = username
        var displayName = string.IsNullOrWhiteSpace(command.DisplayName) ? null : command.DisplayName!.Trim();

        if (!string.IsNullOrWhiteSpace(displayName) && displayName != createdUser.DisplayName)
        {
            users.ChangeDisplayName(createdUser.Username, displayName);
            createdUser.DisplayName = displayName;
        }

        // Хз, как возвращать SessionId пусть пока будет так
        var registeredResult = new RegisterUserResult(createdUser, 1);
        return Task.FromResult(Result<RegisterUserResult>.Ok(registeredResult));
    }
}