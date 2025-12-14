using Api.Application.Common.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Entities;

namespace Api.Application.Features.Tasks.GetTask;

public class GetTaskHandler(IUsersTasksTable tasks) : IRequestHandler<GetTaskQuery, TaskModel>
{
    public async Task<TaskModel> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await tasks.GetTaskFullInfo(request.TaskId);
        return task ??
               throw new ApiException(StatusCodes.Status404NotFound,
                   $"Task {request.TaskId} for {request.Username} not found");
    }
}