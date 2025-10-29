using Api.Application.Common.Exceptions;
using Api.Application.Features.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Features.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TasksController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get information about task
    /// </summary>
    [HttpGet("{username}/{taskId}")]
    public async Task<IActionResult> GetTask(string username, string taskId)
    {
        var result = (await mediator.Send(new GetTaskQuery(username, taskId))).EnsureSuccess();

        return Ok(result);
    }
}