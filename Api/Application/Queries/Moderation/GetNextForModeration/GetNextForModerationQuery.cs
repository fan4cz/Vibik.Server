using MediatR;
using Task = Shared.Models.Task;

namespace Api.Application.Queries.Moderation.GetNextForModeration;

public record GetNextForModerationQuery : IRequest<Task>;