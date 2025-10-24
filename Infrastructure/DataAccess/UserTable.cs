using Shared.Models;
using Infrastructure.Interfaces;

namespace Infrastructure.DataAccess;

public class UserTable : IUserTable
{
    public bool TryGetUser(string username, out User user)
    {
        throw new NotImplementedException();
    }
    
    public bool TryRegisterUser(string username, string hashPassword, out User user)
    {
        throw new NotImplementedException();
    }

    public bool CheckPassword(string username, string hashPassword)
    {
        throw new NotImplementedException();
    }
    
    public User GetUser(string username)
    {
        throw new NotImplementedException();
    }
    
    public int GetUserExp(string username)
    {
        throw new NotImplementedException();
    }

    public bool ChangeDisplayName(string username, string newDisplayName)
    {
        throw new NotImplementedException();
    }
}