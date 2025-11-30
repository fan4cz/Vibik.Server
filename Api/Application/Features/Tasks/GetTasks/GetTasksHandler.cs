using Infrastructure.Interfaces;
using MediatR;
using Shared.Models;
using Shared.Models.Entities;

namespace Api.Application.Features.Tasks.GetTasks;

public class GetTasksHandler(IUsersTasksTable tasks) : IRequestHandler<GetTasksQuery, List<TaskModel>>
{
    public async Task<List<TaskModel>> Handle(GetTasksQuery request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"username: {request.Username}");
        var tasksList = await tasks.GetListActiveUserTasks(request.Username);
        return tasksList;
    }
}