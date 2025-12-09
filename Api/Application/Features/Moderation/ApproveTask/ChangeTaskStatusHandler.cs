using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation.ApproveTask;

public class ChangeTaskStatusHandler(IUsersTasksTable tasks, IUserTable users) : IRequestHandler<ChangeTaskStatusQuery, bool>
{
    public async Task<bool> Handle(ChangeTaskStatusQuery request, CancellationToken cancellationToken)
    {
        //tasks.ChangeModerationStatus(request.userTaskId, request.status);
        if (request.Status == ModerationStatus.Approved)
        {
            await tasks.SetCompleted(request.UserTaskId);
            await users.ChangeExperience(request.UserTaskId, 1);
            await users.TryChangeLevel(request.UserTaskId);
        }
        return await tasks.ChangeModerationStatus(request.UserTaskId,request.Status);
    }
}