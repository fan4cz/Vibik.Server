using Api.Application.Features.Users.GetUser;
using Api.Application.Features.Users.Register;
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
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var result = await mediator.Send(new RegisterUserCommand(req.Username, req.DisplayName, req.Password));
        //Todo: в result возвращается еще и SessionId, насколько я понимаю,
        //так что надо будет в куки сохранять или еще что-то с ним делать, пока не понимаю


        return CreatedAtAction(nameof(GetUser),new {username = result.User.Username}, result.User);
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