using MediatR;
using Shared.Models.Entities;

namespace Api.Application.Features.Tasks.GetTasks;

public record GetTasksQuery(string Username) : IRequest<List<TaskModel>>;