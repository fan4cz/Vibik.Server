using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation.GetModerationStatus;

public class GetModerationStatusHandler(IUsersTasksTable tasks) : IRequestHandler<GetModerationStatusQuery, ModerationStatus>
{
    public async Task<ModerationStatus> Handle(GetModerationStatusQuery request, CancellationToken cancellationToken)
    {
        return await tasks.GetModerationStatus(request.UserTaskId);
    }
}