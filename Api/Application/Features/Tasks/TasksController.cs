using Api.Application.Features.Tasks.GetCompletedTasks;
using Api.Application.Features.Tasks.GetTask;
using Api.Application.Features.Tasks.GetTasks;
using Api.Application.Features.Tasks.SubmitTask;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Application.Features.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TasksController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get information about task
    /// </summary>
    [HttpGet("get_task/{taskId}")]
    [Authorize]
    public async Task<IActionResult> GetTask(string taskId)
    {
        var username = User.FindFirst("username")?.Value;

        if (username is null)
            return Unauthorized();
        var result = await mediator.Send(new GetTaskQuery(username, taskId));

        return Ok(result);
    }

    /// <summary>
    /// Get all user tasks
    /// </summary>
    [HttpGet("get_all")]
    [Authorize]
    public async Task<IActionResult> GetTasks()
    {
        var username = User.FindFirst("username")?.Value;

        if (username is null)
            return Unauthorized();
        var result = await mediator.Send(new GetTasksQuery(username));

        return Ok(result);
    }

    /// <summary>
    /// submit a task
    /// </summary>
    [HttpPost("submit/{taskId}")]
    [Authorize]
    public async Task<IActionResult> SubmitTask(string taskId, [FromForm] List<IFormFile> files)
    {
        var username = User.FindFirst("username")?.Value;
        
        if (username is null)
            return Unauthorized();
        var result = await mediator.Send(new SubmitTaskQuery(username, taskId, files));
        
        return Ok(result);
    }

    /// <summary>
    /// get all tasks completed by a user
    /// </summary>
    [HttpGet("get_completed")]
    [Authorize]
    public async Task<IActionResult> GetCompleted()
    {
        var username = User.FindFirst("username")?.Value;
        if (username is null)
            return Unauthorized();
        
        var result = await mediator.Send(new GetCompletedTasksQuery(username));
        
        return Ok(result);
    }
}