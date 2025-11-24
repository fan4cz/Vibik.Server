using MediatR;
using Shared.Models;

namespace Api.Application.Features.Tasks.GetCompletedTasks;

public record GetCompletedTasksQuery(string Username) : IRequest<List<TaskModel>>;