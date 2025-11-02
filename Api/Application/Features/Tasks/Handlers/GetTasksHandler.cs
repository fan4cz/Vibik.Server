using Api.Application.Common.Results;
using Api.Application.Features.Tasks.Queries;
using Infrastructure.Interfaces;
using MediatR;

namespace Api.Application.Features.Tasks.Handlers;

public class GetTasksHandler(IUsersTasksTable tasks) : IRequestHandler<GetTasksQuery, Result<List<Shared.Models.Task>>>
{
    public async Task<Result<List<Shared.Models.Task>>> Handle(GetTasksQuery request,
        CancellationToken cancellationToken)
    {
        var tasksList = await tasks.GetListActiveUserTasks(request.Username);
        return Result<List<Shared.Models.Task>>.Ok(tasksList);
    }
}