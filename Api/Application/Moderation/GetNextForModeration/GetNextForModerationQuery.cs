using MediatR;
using Task = Shared.Models.Task;

namespace Api.Application.Moderation.GetNextForModeration;

public record GetNextForModerationQuery : IRequest<Task>;