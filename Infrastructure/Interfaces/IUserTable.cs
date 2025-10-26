using Shared.Models;

namespace Infrastructure.Interfaces;

public interface IUserTable
{
    // Извините, я тут поменяла, мы вроде так решили делать, если нет, вернем
    public User? RegisterUser(string username, string hashPassword);
    public bool CheckPassword(string username, string hashPassword);
    public User? GetUser(string username);
    public int GetUserExp(string username);
    public bool ChangeDisplayName(string username, string newDisplayName);
}