using MediatR;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Features.Moderation.CheckModerator;

public class CheckModeratorHandler : IRequestHandler<CheckModeratorQuery, bool>
{
    public Task<bool> Handle(CheckModeratorQuery request, CancellationToken cancellationToken)
        => Task.FromResult(true);
}