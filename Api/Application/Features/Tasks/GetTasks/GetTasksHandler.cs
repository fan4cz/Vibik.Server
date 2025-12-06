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
        var username = request.Username;
        var tasksList = await tasks.GetListActiveUserTasks(username);

        // while (tasksList.Count != 4)
        // {
        //     var task = await tasks.AddUserTask(username);
        //     tasksList.Add(task);
        // }
        
        return tasksList;
    }
}