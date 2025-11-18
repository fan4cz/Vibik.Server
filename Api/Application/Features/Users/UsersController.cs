using Api.Application.Features.Users.GetUser;
using Api.Application.Features.Users.Register;
using Api.Application.Features.Users.Login;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

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