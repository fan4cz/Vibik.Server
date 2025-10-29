using Api.Application.Common.Results;
using MediatR;
using Shared.Models;

namespace Api.Application.Features.Users.Queries;

public record GetUserQuery(string Username) : IRequest<Result<User>>;