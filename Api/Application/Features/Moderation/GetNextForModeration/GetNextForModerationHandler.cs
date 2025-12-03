<<<<<<< HEAD
﻿using Api.Application.Common.Exceptions;
using MediatR;
=======
﻿using Infrastructure.Interfaces;
using MediatR;
using Shared.Models;
>>>>>>> 30cdfc3d1a107bb3ff61b46346f85389ce835890
using Shared.Models.Entities;
using Infrastructure.Interfaces;

namespace Api.Application.Features.Moderation.GetNextForModeration;

public class GetNextForModerationHandler(IUsersTasksTable tasks) : IRequestHandler<GetNextForModerationQuery, ModerationTask>
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
                    [new Uri("https://picsum.photos/seed/moderation/400/300")],
                ExamplePhotos = []
            }
        };
        
        return await Task.FromResult(mockTask);
    }
}