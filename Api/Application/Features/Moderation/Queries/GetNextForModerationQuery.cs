using MediatR;
using Task = Shared.Models.Task;

namespace Api.Application.Features.Moderation.Queries;

public record GetNextForModerationQuery : IRequest<Task>;