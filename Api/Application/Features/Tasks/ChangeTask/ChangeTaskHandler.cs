using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Api.Application.Features.Tasks.ChangeTask;

public class ChangeTaskHandler(IUsersTasksTable tasks, IUserTable users, IMetricsTable metrics)
    : IRequestHandler<ChangeTaskQuery, TaskModel>
{
    private const double Coefficient = 0.2;

    public async Task<TaskModel> Handle(ChangeTaskQuery request, CancellationToken cancellationToken)
    {
        var username = request.Username;
        var taskId = request.TaskId;

        var newTask = await tasks.ChangeUserTask(taskId);
        var reward = tasks.GetReward(taskId);
        await users.ChangeMoney(taskId, (int)(reward.Result * Coefficient));

        await metrics.AddRecord(username, MetricType.Change);
        return newTask;
    }
}