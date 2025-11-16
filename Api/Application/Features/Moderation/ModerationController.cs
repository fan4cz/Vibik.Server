using System.Diagnostics;
using Api.Application.Features.Moderation.CheckModerator;
using Api.Application.Features.Moderation.GetNextForModeration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Features.Moderation;

[ApiController]
[Route("api/[controller]")]
public class ModerationController(IMediator mediator,IConfiguration configuration, ILogger<ModerationController> logger) : ControllerBase
{
    private bool IsValidBot()
    {
        logger.LogInformation("aboba");
        if (!Request.Headers.TryGetValue("X-Bot-Token", out var token))
            return false;

        var expected = configuration["BotAuth:Token"];
        logger.LogInformation("BotAuth:Token from config = '{Token}'", expected);
        return !string.IsNullOrEmpty(expected) && token == expected;
    }
    /// <summary>
    /// Receives the first unmoderated task
    /// </summary>
    [HttpGet("next")]
    public async Task<IActionResult> GetNextForModeration([FromHeader(Name = "X-Session-Id")] int? sessionId)
    {
        if (!IsValidBot())
            return Unauthorized(new { error = "Invalid bot token" });

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
    public async Task<IActionResult> CheckModerator(long tgUserId)
    {
        if (!IsValidBot())
            return Unauthorized(new { error = "Invalid bot token" });

        if (tgUserId == -1)
            return BadRequest("Отсутствует tgUserId");
        
        var result = await mediator.Send(new CheckModeratorQuery(tgUserId));
        return Ok(result);
    }
}