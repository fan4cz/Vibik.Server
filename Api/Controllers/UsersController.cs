using Api.Application.Commands.Users.RegisterUser;
using Api.Application.Common.Exceptions;
using Api.Application.Queries.Tasks.GetTask;
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
        var result = (await mediator.Send(new RegisterUserCommand(req.Username, req.DisplayName, req.Password)))
            .EnsureSuccess();

        //Todo: в result возвращается еще и SessionId, насколько я понимаю,
        //так что надо будет в куки сохранять или еще что-то с ним делать, пока не понимаю

        return StatusCode(StatusCodes.Status201Created, result.User);
    }

    /// <summary>
    /// Get information about a user
    /// </summary>
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        var result = (await mediator.Send(new GetUserQuery(username))).EnsureSuccess();

        return StatusCode(StatusCodes.Status200OK, result);
    }

    /// <summary>
    /// Get information about task
    /// </summary>
    [HttpGet("{username}/tasks/{taskId}")]
    public async Task<IActionResult> GetTask(string username, string taskId)
    {
        var result = (await mediator.Send(new GetTaskQuery(username, taskId))).EnsureSuccess();

        return StatusCode(StatusCodes.Status200OK, result);
    }

    public sealed class RegisterRequest
    {
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string? DisplayName { get; init; } = null!;
    }
}