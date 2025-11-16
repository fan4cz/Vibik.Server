using Api.Application.Features.Tasks.GetTask;
using Api.Application.Features.Tasks.GetTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<IActionResult> GetTask(string username, string taskId)
    {
        var result = await mediator.Send(new GetTaskQuery(username, taskId));

        return Ok(result);
    }

    /// <summary>
    /// Get all user tasks
    /// </summary>
    [HttpGet("{username}")]
    [Authorize]
    public async Task<IActionResult> GetTasks( string username)
    {
        var result = await mediator.Send(new GetTasksQuery(username));

        return Ok(result);
    }
}