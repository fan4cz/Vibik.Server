using Shared.Models;

namespace Infrastructure.Interfaces;

public interface IUserTable
{    
    public User? RegisterUser(string username, string hashPassword)
    {
        throw new NotImplementedException();
    }

    public bool CheckPassword(string username, string hashPassword)
    {
        throw new NotImplementedException();
    }
    
    public User? GetUser(string username)
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