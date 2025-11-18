using MediatR;
using Shared.Models;

namespace Api.Application.Features.Tasks.GetTasks;

public record GetTasksQuery(string Username) : IRequest<List<TaskModel>>;