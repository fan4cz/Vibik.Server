using MediatR;
using Shared.Models;

namespace Api.Application.Moderation.Queries.GetNextForModeration;

public record GetNextForModerationQuery(int Session) : IRequest<PhotoModel>;