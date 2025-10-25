using MediatR;
using Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Moderation.Queries.GetCheckModerator;

public class GetCheckModeratorHandler : IRequestHandler<GetCheckModeratorQuery, bool>
{
    public Task<bool> Handle(GetCheckModeratorQuery request, CancellationToken cancellationToken)
        => Task.FromResult(true);
}