using Api.Application.Moderation.GetNextForModeration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers;

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
        if (sessionId == null)
            return Unauthorized(new { error = "Session inactive" });
        
        var result = await mediator.Send(new GetNextForModerationQuery());
        return Ok(result);
    }
    [HttpGet("{tgUserId:long}/check")]
    public async Task<IActionResult> CheckModerator([FromQuery] int tgUserId)
    {
        if (tgUserId == -1)
            return BadRequest("Отсутствует tgUserId");
        
        var result = await mediator.Send(new GetCheckModeratorQuery(tgUserId));
        return Ok(result);
    }
}