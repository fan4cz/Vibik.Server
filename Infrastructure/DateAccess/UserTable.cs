using Shared.Models;

namespace Infrastructure.Interfaces;

public class UserTable
{
    public static bool TryGetUser(string username, out User user)
    {
        throw new NotImplementedException();
    }
    
    public static bool TryRegisterUser(string username, string hashPassword, out User user)
    {
        throw new NotImplementedException();
    }

    public static bool CheckPassword(string username, string hashPassword)
    {
        throw new NotImplementedException();
    }
    
    public static User GetUser(string username)
    {
        throw new NotImplementedException();
    }
    
    public static int GetUserExp(string username)
    {
        throw new NotImplementedException();
    }

    public static bool ChangeDisplayName(string username, string newDisplayName)
    {
        throw new NotImplementedException();
    }
}