using Api.Application.Features.Photos.UploadPhoto;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Tasks.SubmitTask;

public class SubmitTaskHandler(IUsersTasksTable tasks,IMetricsTable metrics, IMediator mediator)
    : IRequestHandler<SubmitTaskQuery, List<string>>
{
    public async Task<List<string>> Handle(SubmitTaskQuery request, CancellationToken cancellationToken)
    {
        var uploadedNames = new List<string>();
        var username = request.Username;
        var taskId = request.TaskId;

        foreach (var file in request.Files)
        {
            var name = await mediator.Send(new UploadPhotoCommand(file), cancellationToken);
            await tasks.AddPhoto(username, taskId, name);
            uploadedNames.Add(name);
        }
        
        await tasks.ChangeModerationStatus(username, taskId, ModerationStatus.Waiting);

        await metrics.AddRecord(username, MetricType.Submit);
        return uploadedNames;
    }
}