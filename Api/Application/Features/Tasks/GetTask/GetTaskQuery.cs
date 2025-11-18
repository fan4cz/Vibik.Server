using MediatR;
using Shared.Models;

namespace Api.Application.Features.Tasks.GetTask;

public record GetTaskQuery(string Username, string TaskId) : IRequest<TaskModel>;