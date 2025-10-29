using Api.Application.Features.Moderation.Queries;
using MediatR;
using Shared.Models;
using Task = Shared.Models.Task;

namespace Api.Application.Features.Moderation.Handlers;

public class GetNextForModerationHandler : IRequestHandler<GetNextForModerationQuery, Task>
{
    public async Task<Task> Handle(GetNextForModerationQuery query, CancellationToken cancellationToken)
    {
        var mockTask = new Task
        {
            TaskId = "honey_cars",
            Name = "Медовые машины",
            StartTime = DateTime.Now,
            Reward = 10,
            ExtendedInfo = new TaskExtendedInfo
            {
                Description = "Сфоткать 3 желтые машины",
                PhotosRequired = 3,
                UserPhotos = [new PhotoModel { Url = "https://picsum.photos/seed/moderation/400/300" }]
            }
        };

        return await System.Threading.Tasks.Task.FromResult(mockTask);
    }
}