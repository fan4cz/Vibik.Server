using System.Diagnostics;
using Api.Application.Features.Moderation.CheckModerator;
using Api.Application.Features.Moderation.GetNextForModeration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Features.Moderation;

[ApiController]
[Route("api/[controller]")]
public class ModerationController(
    IMediator mediator,
    IConfiguration configuration,
    ILogger<ModerationController> logger) : ControllerBase
{
    /// <summary>
    /// Receives the first unmoderated task
    /// </summary>
    [HttpGet("next")]
    [Authorize]
    public async Task<IActionResult> GetNextForModeration()
    {
        //TODO: ну это надо бы в идеале в какой-то там Middlware вынести
        // if (sessionId == null)
        //     return Unauthorized(new { error = "Session inactive" });

        var result = await mediator.Send(new GetNextForModerationQuery());
        return Ok(result);
    }

    /// <summary>
    /// checking that the user is a moderator
    /// </summary>
    [HttpGet("{tgUserId:long}/check")]
    [Authorize]
    public async Task<IActionResult> CheckModerator(long tgUserId)
    {
        if (tgUserId == -1)
            return BadRequest("Отсутствует tgUserId");

        var result = await mediator.Send(new CheckModeratorQuery(tgUserId));
        return Ok(result);
    }
}