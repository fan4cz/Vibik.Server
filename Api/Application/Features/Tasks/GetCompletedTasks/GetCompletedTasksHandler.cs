using Infrastructure.Interfaces;
using MediatR;
using Shared.Models;

namespace Api.Application.Features.Tasks.GetCompletedTasks;

public class GetCompletedTasksHandler(IUsersTasksTable tasks) : IRequestHandler<GetCompletedTasksQuery, List<TaskModel>>
{
    public async Task<List<TaskModel>> Handle(GetCompletedTasksQuery request, CancellationToken cancellationToken)
    {
        var username =  request.Username;
        return await tasks.GetUserSubmissionHistory(username);
    }
}