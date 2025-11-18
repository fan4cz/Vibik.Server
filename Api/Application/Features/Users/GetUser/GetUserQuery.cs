using MediatR;
using Shared.Models;

namespace Api.Application.Features.Users.GetUser;

public record GetUserQuery(string Username) : IRequest<User>;