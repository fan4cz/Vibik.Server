using Infrastructure.Interfaces;
using MediatR;
using Shared.Models;

namespace Api.Application.Features.Tasks.GetTasks;

public class GetTasksHandler(IUsersTasksTable tasks) : IRequestHandler<GetTasksQuery, List<TaskModel>>
{
    public async Task<List<TaskModel>> Handle(GetTasksQuery request,
        CancellationToken cancellationToken)
    {
        var tasksList = await tasks.GetListActiveUserTasks(request.Username);
        return tasksList;
    }
}