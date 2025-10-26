using MediatR;
using Shared.Models;

namespace Api.Application.Moderation.Queries.GetCheckModerator;

public record GetCheckModeratorQuery(int TgUserId) : IRequest<bool>;