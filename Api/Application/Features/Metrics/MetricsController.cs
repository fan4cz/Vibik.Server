using Api.Application.Features.Metrics.GetMetrics;

namespace Api.Application.Features.Metrics;
using System.Diagnostics;
using Api.Application.Features.Moderation.ApproveTask;
using Api.Application.Features.Moderation.CheckModerator;
using Api.Application.Features.Moderation.GetModerationStatus;
using Api.Application.Features.Moderation.GetNextForModeration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Enums;
[ApiController]
[Route("api/")]
public class MetricsController(
    IMediator mediator,
    IConfiguration configuration,
    ILogger<MetricsController> logger) : ControllerBase{
    
    [HttpGet("metrics")]
    [Authorize(Roles = UserRoleNames.TgBot)]
    public async Task<IActionResult> GetMetrics()
    {

        var result = await mediator.Send(new GetMetricsQuery());
        return Ok(result);
    }
}

    
