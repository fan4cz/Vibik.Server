using Api.Application.Common.Results;
using MediatR;
using Shared.Models;

namespace Api.Application.Queries.Users.GetUser;

public record GetUserQuery(string Username) : IRequest<Result<User>>;