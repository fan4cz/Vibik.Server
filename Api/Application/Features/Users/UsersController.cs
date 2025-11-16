using Api.Application.Features.Users.GetUser;
using Api.Application.Features.Users.Register;
using Api.Application.Features.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Application.Features.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// User registration
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var result = await mediator.Send(new RegisterUserCommand(req.Username, req.DisplayName, req.Password));
        //Todo: в result возвращается еще и SessionId, насколько я понимаю,
        //так что надо будет в куки сохранять или еще что-то с ним делать, пока не понимаю
        if (!result.Status)
            return NoContent();

        return Ok();
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var result = await mediator.Send(new LoginUserCommand(req.Username,  req.Password));

        return Ok(result.JwtToken);
    }

    /// <summary>
    /// Get information about a user
    /// </summary>
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        var result = await mediator.Send(new GetUserQuery(username));

        return Ok(result);
    }
}