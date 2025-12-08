using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Api.Application.Features.Tasks.ChangeTask;

public class ChangeTaskHandler(IUsersTasksTable tasks, IMetricsTable metrics) : IRequestHandler<ChangeTaskQuery, TaskModel>
{
        public async Task<TaskModel> Handle(ChangeTaskQuery request, CancellationToken cancellationToken)
        {
            var username = request.Username;

                var newTask = await tasks.ChangeUserTask(request.TaskId);
                await metrics.AddRecord(username, MetricType.Change);
                return newTask;
        }
}