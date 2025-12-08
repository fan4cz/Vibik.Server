using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Entities;

namespace Api.Application.Features.Moderation.GetNextForModeration;

public class GetNextForModerationHandler(IUsersTasksTable tasks)
    : IRequestHandler<GetNextForModerationQuery, ModerationTask?>
{
    public async Task<ModerationTask?> Handle(GetNextForModerationQuery query, CancellationToken cancellationToken)
    {
        return await tasks.GetModerationTask();
    }
}