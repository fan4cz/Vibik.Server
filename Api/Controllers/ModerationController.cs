using Api.Application.Moderation.Queries.GetNextForModeration;
using Api.Application.Moderation.Queries.GetCheckModerator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModerationController(IMediator mediator) : ControllerBase
{
    [HttpGet("next")]
    public async Task<IActionResult> GetNextForModeration([FromQuery] int session)
    {
        if (session == -1)
            return BadRequest("Отсутствует сессия");
        
        var result = await mediator.Send(new GetNextForModerationQuery(session));
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
