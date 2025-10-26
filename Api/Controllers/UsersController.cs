using Api.Application.Commands.Users.RegisterUser;
using Api.Application.Queries.Users.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// User registration
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var result = await mediator.Send(new RegisterUserCommand(req.Username, req.DisplayName, req.Password));

        if (!result.IsSuccess)
            return result.Error!.Value.Code switch
            {
                "username_taken" => Conflict(new { error = "username_taken" }),
                "validation" => UnprocessableEntity(new { error = result.Error.Value.Message }),
                _ => Problem(statusCode: 500)
            };

        //Todo: в result возвращается еще и SessionId, насколько я понимаю,
        //так что надо будет в куки сохранять или еще что-то с ним делать, пока не понимаю

        return StatusCode(StatusCodes.Status201Created, result.Value?.User);
    }

    /// <summary>
    /// Get information about a user
    /// </summary>
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        var result = await mediator.Send(new GetUserQuery(username));

        if (!result.IsSuccess)
            return result.Error!.Value.Code switch
            {
                "not_found" => NotFound(new { error = "not_found" }),
                _ => Problem(statusCode: 500)
            };

        return StatusCode(StatusCodes.Status200OK, result.Value);
    }

    public sealed class RegisterRequest
    {
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string? DisplayName { get; init; } = null!;
    }
}