using Api.Application.Features.Users.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Features.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get information about a user
    /// </summary>
    [HttpGet("get_user")]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var username = User.FindFirst("username")?.Value;
        //var username = "string";
        if (username is null)
            return Unauthorized();

        var result = await mediator.Send(new GetUserQuery(username));
        return Ok(result);
    }
}