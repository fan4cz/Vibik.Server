using MediatR;
using Shared.Models;

namespace Api.Application.Features.Moderation.GetNextForModeration;

public class GetNextForModerationHandler : IRequestHandler<GetNextForModerationQuery, ModerationTask>
{
    public async Task<ModerationTask> Handle(GetNextForModerationQuery query, CancellationToken cancellationToken)
    {
        var mockTask = new ModerationTask
        {
            UserTaskId = 1,
            TaskId = "honey_cars",
            Name = "Медовые машины",
            Tags = ["я", "хз"],
            ExtendedInfo = new TaskModelExtendedInfo
            {
                Description = "Сфоткать 3 желтые машины",
                PhotosRequired = 3,
                UserPhotos =
                [
                    new Uri("https://picsum.photos/seed/moderation/400/300")
                ],
                ExamplePhotos = []
            }
        };

        return await Task.FromResult(mockTask);
    }
}