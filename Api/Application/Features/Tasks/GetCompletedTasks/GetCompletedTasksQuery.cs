using MediatR;
using Shared.Models.Entities;

namespace Api.Application.Features.Tasks.GetCompletedTasks;

public record GetCompletedTasksQuery(string Username) : IRequest<List<TaskModel>>;