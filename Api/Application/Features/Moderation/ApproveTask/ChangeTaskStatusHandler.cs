using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation.ApproveTask;

public class ChangeTaskStatusHandler(IUsersTasksTable tasks, IUserTable users) : IRequestHandler<ChangeTaskStatusQuery, bool>
{
    public async Task<bool> Handle(ChangeTaskStatusQuery request, CancellationToken cancellationToken)
    {
        var userTaskId = request.UserTaskId;
        
        if (request.Status == ModerationStatus.Approved)
        {
            var reward = await tasks.GetReward(userTaskId);
            
            await tasks.SetCompleted(userTaskId);
            await users.ChangeExperience(userTaskId, 1);
            await users.TryChangeLevel(userTaskId);
            await users.ChangeMoney(userTaskId, reward);
        }
        return await tasks.ChangeModerationStatus(userTaskId, request.Status);
    }
}