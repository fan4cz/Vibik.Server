using Api.Application.Common.Results;
using Infrastructure.Interfaces;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Queries.Tasks.GetTask;

// Ну здесь как будто бы не IUserTaskTable должен быть а ITaskTable у нас же одна общая таблица
public class GetTaskHandler(IUserTaskTable tasks) : IRequestHandler<GetTaskQuery, Result<Shared.Models.Task>>
{
    public Task<Result<Shared.Models.Task>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = tasks.GetTaskFullInfo(request.TaskId, request.Username);
        return Task.FromResult(task is null
            ? Result<Shared.Models.Task>.Fail("not_found", "Task not found")
            : Result<Shared.Models.Task>.Ok(task));
    }
}