using MediatR;

namespace Api.Application.Features.Tasks.ChangeTask;

public record ChangeTaskQuery(string Username, string TaskId) : IRequest<bool>;