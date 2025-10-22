using MediatR;
using Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Moderation.Queries.GetNextForModeration;

public class GetNextForModerationHandler : IRequestHandler<GetNextForModerationQuery, PhotoModel>
{
    public async Task<PhotoModel> Handle(GetNextForModerationQuery request, CancellationToken cancellationToken)
    {
        var mockPhoto = new PhotoModel { Url = "https://picsum.photos/seed/moderation/400/300" };
        
        return await Task.FromResult(mockPhoto);
    }
}