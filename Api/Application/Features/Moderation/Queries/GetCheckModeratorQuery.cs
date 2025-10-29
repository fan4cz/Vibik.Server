using MediatR;

namespace Api.Application.Features.Moderation.Queries;

public record GetCheckModeratorQuery(long TgUserId) : IRequest<bool>;