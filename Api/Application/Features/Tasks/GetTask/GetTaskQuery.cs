using MediatR;
using Shared.Models;
using Shared.Models.Entities;

namespace Api.Application.Features.Tasks.GetTask;

public record GetTaskQuery(string Username, string TaskId) : IRequest<TaskModel>;