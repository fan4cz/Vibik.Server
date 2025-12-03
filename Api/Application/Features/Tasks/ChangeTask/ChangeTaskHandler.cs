using Infrastructure.Interfaces;
using MediatR;

namespace Api.Application.Features.Tasks.ChangeTask;

// TODO: вместо bool как будто бы хочется TaskModel возвращать
public class ChangeTaskHandler(IUsersTasksTable tasks) : IRequestHandler<ChangeTaskQuery, bool>
{
    public async Task<bool> Handle(ChangeTaskQuery request, CancellationToken cancellationToken)
    {
        var username = request.Username;

        var newTask = await tasks.AddUserTask(username);

        return newTask;
    }
}