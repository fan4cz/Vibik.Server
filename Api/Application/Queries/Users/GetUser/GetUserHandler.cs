using Api.Application.Common.Results;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace Api.Application.Queries.Users.GetUser;

public class GetUserHandler(IUserTable users) : IRequestHandler<GetUserQuery, Result<User>>
{
    public async Task<Result<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await users.GetUser(request.Username);
        return user is null
            ? Result<User>.Fail("not_fount", "User not found")
            : Result<User>.Ok(user);
    }
}