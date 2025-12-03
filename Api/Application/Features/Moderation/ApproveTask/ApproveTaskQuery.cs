using MediatR;

namespace Api.Application.Features.Moderation.ApproveTask;

public record ApproveTaskQuery(int userTaskId) : IRequest<bool>;