using Api.Application.Moderation.Queries.GetNextForModeration;
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
}