using MediatR;
using Shared.Models;
using Shared.Models.Entities;

namespace Api.Application.Features.Moderation.GetNextForModeration;

public record GetNextForModerationQuery : IRequest<ModerationTask?>;