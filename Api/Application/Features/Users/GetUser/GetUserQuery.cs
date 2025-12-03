using MediatR;
using Shared.Models;
using Shared.Models.Entities;

namespace Api.Application.Features.Users.GetUser;

public record GetUserQuery(string Username) : IRequest<User>;