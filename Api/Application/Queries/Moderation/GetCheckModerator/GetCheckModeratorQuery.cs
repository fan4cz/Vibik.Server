using MediatR;

namespace Api.Application.Queries.Moderation.GetCheckModerator;

public record GetCheckModeratorQuery(long TgUserId) : IRequest<bool>;