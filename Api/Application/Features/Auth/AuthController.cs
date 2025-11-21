using Api.Application.Features.Auth.Login;
using Api.Application.Features.Auth.Refresh;
using Api.Application.Features.Auth.Register;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Application.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
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

        return Created();
    }

    /// <summary>
    /// User login
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var result = await mediator.Send(new LoginUserCommand(req.Username, req.Password));

        return Ok(result);
    }

    /// <summary>
    /// Refresh access and refresh token
    /// </summary>
    [HttpPost("refresh")]
    [Authorize(Policy = AuthPolicies.RefreshTokenOnly)]
    public async Task<IActionResult> Refresh()
    {
        var username = User.FindFirst("username")?.Value;
        if (string.IsNullOrEmpty(username))
            return Unauthorized();
        var result = await mediator.Send(new RefreshCommand(username));

        return Ok(result);
    }
}