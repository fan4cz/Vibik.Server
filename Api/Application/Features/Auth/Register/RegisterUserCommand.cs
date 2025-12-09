using MediatR;
using Shared.Models.DTO.Response;

namespace Api.Application.Features.Auth.Register;

public record RegisterUserCommand(
    string Username,
    string? DisplayName,
    string Password
) : IRequest<RegisterUserResponse>;