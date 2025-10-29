using Api.Application.Features.Moderation.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Features.Moderation;

[ApiController]
[Route("api/[controller]")]
public class ModerationController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Receives the first unmoderated task
    /// </summary>
    [HttpGet("next")]
    public async Task<IActionResult> GetNextForModeration([FromHeader(Name = "X-Session-Id")] int? sessionId)
    {
        //TODO: ну это надо бы в идеале в какой-то там Middlware вынести
        if (sessionId == null)
            return Unauthorized(new { error = "Session inactive" });
        
        var result = await mediator.Send(new GetNextForModerationQuery());
        return Ok(result);
    }
    
    /// <summary>
    /// checking that the user is a moderator
    /// </summary>
    [HttpGet("{tgUserId:long}/check")]
    public async Task<IActionResult> CheckModerator(long tgUserId)
    {
        if (tgUserId == -1)
            return BadRequest("Отсутствует tgUserId");
        
        var result = await mediator.Send(new GetCheckModeratorQuery(tgUserId));
        return Ok(result);
    }
}