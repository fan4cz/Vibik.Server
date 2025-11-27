using MediatR;

namespace Api.Application.Features.Moderation.ApproveTask;

public class ApproveTaskHandler : IRequestHandler<ApproveTaskQuery, bool>
{
    public Task<bool> Handle(ApproveTaskQuery request, CancellationToken cancellationToken)
        => Task.FromResult(true);
}