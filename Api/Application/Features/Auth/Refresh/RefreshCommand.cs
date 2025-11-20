using MediatR;
using Shared.Models;

namespace Api.Application.Features.Auth.Refresh;

public record RefreshCommand(
    string Username
) : IRequest<RefreshResponse>;