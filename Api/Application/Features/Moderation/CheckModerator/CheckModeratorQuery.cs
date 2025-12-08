using MediatR;

namespace Api.Application.Features.Moderation.CheckModerator;

public record CheckModeratorQuery(long TaskUserId) : IRequest<bool>;