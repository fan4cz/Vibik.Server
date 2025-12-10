using Infrastructure.Interfaces;
using MediatR;

namespace Api.Application.Features.Moderation.GetModerationStatus;

public class GetModerationStatusHandler(IUsersTasksTable tasks) : IRequestHandler<GetModerationStatusQuery, string>
{
    public async Task<string> Handle(GetModerationStatusQuery request, CancellationToken cancellationToken)
    {
        return await tasks.GetModerationStatus(request.UserTaskId);
    }
}