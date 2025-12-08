using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation.ApproveTask;

public class ChangeTaskStatusHandler(IUsersTasksTable tasks) : IRequestHandler<ChangeTaskStatusQuery, bool>
{
    public async Task<bool> Handle(ChangeTaskStatusQuery request, CancellationToken cancellationToken)
    {
        //tasks.ChangeModerationStatus(request.userTaskId, request.status);
        if(request.status == ModerationStatus.Approved)
            await tasks.SetCompleted(request.userTaskId);
        return await tasks.ChangeModerationStatus(request.userTaskId,request.status);
    }
}