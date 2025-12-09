using Api.Application.Features.Metrics.GetMetrics;

namespace Api.Application.Features.Metrics;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Enums;
[ApiController]
[Route("api/")]
public class MetricsController(
    IMediator mediator,
    ILogger<MetricsController> logger) : ControllerBase{
    
    [HttpGet("metrics")]
    [Authorize(Roles = UserRoleNames.TgBot)]
    public async Task<IActionResult> GetMetrics()
    {

        var result = await mediator.Send(new GetMetricsQuery());
        return Ok(result);
    }
}

    
