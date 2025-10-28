using Api.Application.Commands.Users.RegisterUser;
using Api.Application.Common.Results;

namespace Api.Application.Commands.Users.Validation;

public class RegisterUserValidation
{
    public static Result Validate(RegisterUserCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            return Result.Fail("validation", "Username is required");
        
        if (string.IsNullOrWhiteSpace(command.Password))
            return Result.Fail("validation", "Password is required");
        
        if (command.Password.Length < 5)
            return Result.Fail("validation", "Password is too short");

        return Result.Ok();
    }
}