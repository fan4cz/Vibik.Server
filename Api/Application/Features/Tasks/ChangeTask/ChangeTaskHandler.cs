using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Tasks.ChangeTask;

// TODO: вместо bool как будто бы хочется TaskModel возвращать
public class ChangeTaskHandler(IUsersTasksTable tasks,IMetricsTable metrics) : IRequestHandler<ChangeTaskQuery, bool>
{
    public async Task<bool> Handle(ChangeTaskQuery request, CancellationToken cancellationToken)
    {
        var username = request.Username;

        var newTask = await tasks.AddUserTask(username);
        metrics.AddRecord(username, MetricType.Change);
        return newTask;
    }
}