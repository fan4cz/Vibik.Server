using Api.Application.Common.Results;
using MediatR;
using Task = Shared.Models.Task;

namespace Api.Application.Features.Tasks.Queries;

public record GetTaskQuery(string Username, string TaskId) : IRequest<Result<Task>>;