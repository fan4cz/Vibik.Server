using MediatR;
using Shared.Models.Entities;

namespace Api.Application.Features.Tasks.ChangeTask;

public record ChangeTaskQuery(string Username, string TaskId) : IRequest<TaskModel>;