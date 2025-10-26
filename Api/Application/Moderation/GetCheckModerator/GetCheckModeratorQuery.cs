using MediatR;

namespace Api.Application.Moderation.GetCheckModerator;

public record GetCheckModeratorQuery(long TgUserId) : IRequest<bool>;