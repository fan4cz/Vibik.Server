using MediatR;
using Shared.Models.Enums;

namespace Api.Application.Features.Moderation.ApproveTask;

public record ChangeTaskStatusQuery(int UserTaskId, ModerationStatus Status) : IRequest<bool>;