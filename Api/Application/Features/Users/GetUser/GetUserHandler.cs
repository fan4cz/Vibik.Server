using Api.Application.Common.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Entities;

namespace Api.Application.Features.Users.GetUser;

public class GetUserHandler(IUserTable users) : IRequestHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await users.GetUser(request.Username);
        return user ?? throw new ApiException(StatusCodes.Status404NotFound, "User not found");
    }
}