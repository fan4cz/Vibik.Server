using Infrastructure.DataAccess;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation.ApproveTask;

public class ChangeTaskStatusHandler(IUsersTasksTable tasks, IUserTable users)
    : IRequestHandler<ChangeTaskStatusQuery, bool>
{
    public async Task<bool> Handle(ChangeTaskStatusQuery request, CancellationToken cancellationToken)
    {
        var user = await users.GetUser(request.UserTaskId);
        var reward = await tasks.GetReward(request.UserTaskId);
        if (request.Status == ModerationStatus.Approved)
        {
            await users.AddMoney(user.Username, reward);
            if ((user.Experience + 1) % 5 == 0)
            {
                await users.AddLevel(user.Username, 1);
                await users.AddExperience(user.Username, -4);
            }
            else
                await users.AddExperience(user.Username, 1);
        }

        return await tasks.ChangeModerationStatus(request.UserTaskId, request.Status);
    }
}