using MediatR;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Queries.Moderation.GetCheckModerator;

public class GetCheckModeratorHandler : IRequestHandler<GetCheckModeratorQuery, bool>
{
    public Task<bool> Handle(GetCheckModeratorQuery request, CancellationToken cancellationToken)
        => Task.FromResult(true);
}