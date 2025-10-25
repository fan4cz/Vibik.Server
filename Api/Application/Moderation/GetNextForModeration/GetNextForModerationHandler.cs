using MediatR;
using Shared.Models;
using Task = Shared.Models.Task;

namespace Api.Application.Moderation.GetNextForModeration;

public class GetNextForModerationHandler : IRequestHandler<GetNextForModerationQuery, Task>
{
    public async Task<Task> Handle(GetNextForModerationQuery query, CancellationToken cancellationToken)
    {
        var mockTask = new Task
        {
            TaskId = "Honey cars",
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