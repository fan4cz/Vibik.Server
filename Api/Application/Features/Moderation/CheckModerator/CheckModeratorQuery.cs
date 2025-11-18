using MediatR;

namespace Api.Application.Features.Moderation.CheckModerator;

public record CheckModeratorQuery(long TgUserId) : IRequest<bool>;