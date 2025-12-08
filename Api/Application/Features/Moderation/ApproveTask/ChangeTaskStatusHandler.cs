using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation.ApproveTask;

public class ChangeTaskStatusHandler(IUsersTasksTable tasks) : IRequestHandler<ChangeTaskStatusQuery, bool>
{
    public Task<bool> Handle(ChangeTaskStatusQuery request, CancellationToken cancellationToken)
    {
        //tasks.ChangeModerationStatus(request.userTaskId, request.status);
        return tasks.ChangeModerationStatus(request.userTaskId,request.status);
    }
}