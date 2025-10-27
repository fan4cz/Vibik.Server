using Shared.Models;

namespace Infrastructure.Interfaces;

public interface IUserTable
{
    public User? RegisterUser(string username, string hashPassword);
    public bool CheckPassword(string username, string hashPassword);
    public User? GetUser(string username);
    public int GetUserExp(string username);
    public bool ChangeDisplayName(string username, string newDisplayName);
}