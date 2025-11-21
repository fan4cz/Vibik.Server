using MediatR;
using Shared.Models;

namespace Api.Application.Features.Moderation.GetNextForModeration;

public record GetNextForModerationQuery : IRequest<ModerationTask>;