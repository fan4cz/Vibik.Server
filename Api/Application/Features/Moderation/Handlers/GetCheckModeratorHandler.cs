using Api.Application.Features.Moderation.Queries;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Features.Moderation.Handlers;

public class GetCheckModeratorHandler : IRequestHandler<GetCheckModeratorQuery, bool>
{
    public Task<bool> Handle(GetCheckModeratorQuery request, CancellationToken cancellationToken)
        => Task.FromResult(true);
}