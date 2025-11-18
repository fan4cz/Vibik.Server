using Api.Application.Features.Users.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Features.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
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