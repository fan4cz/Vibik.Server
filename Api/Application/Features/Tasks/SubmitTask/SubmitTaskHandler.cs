using Api.Application.Features.Photos.UploadPhoto;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Tasks.SubmitTask;

public class SubmitTaskHandler(IUsersTasksTable tasks, IMetricsTable metrics, IMediator mediator)
    : IRequestHandler<SubmitTaskQuery, List<string>>
{
    public async Task<List<string>> Handle(SubmitTaskQuery request, CancellationToken cancellationToken)
    {
        var uploadedNames = new List<string>();
        var username = request.Username;
        var taskId = request.TaskId;
        var files = request.Files;
        foreach (var file in files)
        {
            var name = await mediator.Send(new UploadPhotoCommand(file), cancellationToken);
            uploadedNames.Add(name);
        }

        await tasks.SetPhotos(taskId, uploadedNames.ToArray());

        await tasks.ChangeModerationStatus(taskId, ModerationStatus.Waiting);

        await metrics.AddRecord(username, MetricType.Submit);
        return uploadedNames;
    }
}