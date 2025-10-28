using Api.Application.Common.Results;
using Infrastructure.Interfaces;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Queries.Tasks.GetTask;

public class GetTaskHandler(IUsersTasksTable tasks) : IRequestHandler<GetTaskQuery, Result<Shared.Models.Task>>
{
    public async Task<Result<Shared.Models.Task>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await tasks.GetTaskFullInfo(request.TaskId, request.Username);
        return task is null
            ? Result<Shared.Models.Task>.Fail("not_found", "Task not found")
            : Result<Shared.Models.Task>.Ok(task);
    }
}