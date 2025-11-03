using MediatR;
using Shared.Models;

namespace Api.Application.Features.Moderation.Queries;

public record GetNextForModerationQuery : IRequest<ModerationTask>;