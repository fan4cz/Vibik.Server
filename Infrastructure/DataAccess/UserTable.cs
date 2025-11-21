using Shared.Models;
using Infrastructure.Interfaces;

namespace Infrastructure.DataAccess;

public class UserTable : IUserTable
{
    public async Task<User?> RegisterUser(string username, string hashPassword)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckPassword(string username, string hashPassword)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUser(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetUserExp(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ChangeDisplayName(string username, string newDisplayName)
    {
        throw new NotImplementedException();
    }
}