using Infrastructure.DataAccess;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation.ApproveTask;

public class ChangeTaskStatusHandler(IUsersTasksTable tasks, IUserTable users) : IRequestHandler<ChangeTaskStatusQuery, bool>
{
    public async Task<bool> Handle(ChangeTaskStatusQuery request, CancellationToken cancellationToken)
    {
        var task = await tasks.GetTaskNoExtendedInfo(request.UserTaskId);
        Console.WriteLine($"{task.Name}, {task.UserTaskId}");
        if (request.Status == ModerationStatus.Approved)
        {

            await users.AddExperience(task.Name, 1);
            await users.AddLevel(task.Name, 1);
            await users.AddMoney(task.Name, task.Reward);
        }
        return await tasks.ChangeModerationStatus(request.UserTaskId, request.Status);
    }
}