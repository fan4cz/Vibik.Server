using Api.Application.Features.Moderation.ApproveTask;
using Api.Application.Features.Moderation.CheckModerator;
using Api.Application.Features.Moderation.GetModerationStatus;
using Api.Application.Features.Moderation.GetNextForModeration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation;

[ApiController]
[Route("api/[controller]")]
public class ModerationController(
    IMediator mediator,
    ILogger<ModerationController> logger) : ControllerBase
{
    /// <summary>
    /// Receives the first unmoderated task
    /// </summary>
    [HttpGet("next")]
    [Authorize(Roles = UserRoleNames.TgBot)]
    public async Task<IActionResult> GetNextForModeration()
    {
        var result = await mediator.Send(new GetNextForModerationQuery());
        return Ok(result);
    }

    /// <summary>
    /// checking that the user is a moderator
    /// </summary>
    [HttpGet("{tgUserId:long}/check")]
    [Authorize(Roles = UserRoleNames.TgBot)]
    public async Task<IActionResult> CheckModerator(long tgUserId)
    {
        if (tgUserId == -1)
            return BadRequest("Отсутствует tgUserId");

        var result = await mediator.Send(new CheckModeratorQuery(tgUserId));
        return Ok(result);
    }

    /// <summary>
    /// approve task
    /// </summary>
    [HttpPost("{userTaskId:int}/approve")]
    // [Authorize(Roles = UserRoleNames.TgBot)]
    public async Task<IActionResult> ApproveTask(int userTaskId)
    {
        if (userTaskId == -1)
            return BadRequest("Отсутствует userTaskId");

        var result = await mediator.Send(new ChangeTaskStatusQuery(userTaskId,ModerationStatus.Approved));
        return Ok();
    }
    
    /// <summary>
    /// reject task
    /// </summary>
    [HttpPost("{userTaskId:int}/reject")]
    [Authorize(Roles = UserRoleNames.TgBot)]
    public async Task<IActionResult> RejectTask(int userTaskId)
    {
        if (userTaskId == -1)
            return BadRequest("Отсутствует userTaskId");

        var result = await mediator.Send(new ChangeTaskStatusQuery(userTaskId,ModerationStatus.Reject));
        return Ok();
    }

    [HttpGet("debug-me")]
    [Authorize]
    public IActionResult DebugMe()
    {
        var info = new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            Name = User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList(),
            IsInRole_tg_bot = User.IsInRole(UserRoleNames.TgBot)
        };

        return Ok(info);
    }

    [HttpGet("{userTaskId:int}/get-moderation-status")]
    [Authorize]
    public async Task<IActionResult> GetModerationStatus(int userTaskId)
    {
        if (userTaskId == -1)
            return BadRequest("Отсутствует userTaskId");

        var result = await mediator.Send(new GetModerationStatusQuery(userTaskId));
        return Ok(result);
    }
}