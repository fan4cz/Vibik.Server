using MediatR;

namespace Api.Application.Features.Moderation.GetModerationStatus;

public record GetModerationStatusQuery(int UserTaskId) : IRequest<string>;