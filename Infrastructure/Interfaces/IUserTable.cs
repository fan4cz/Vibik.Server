using Shared.Models;
using Shared.Models.Entities;

namespace Infrastructure.Interfaces;

public interface IUserTable
{
    public Task<User?> RegisterUser(string username, string hashPassword);
    public Task<bool> LoginUser(string username, string hashPassword);
    public Task<User?> GetUser(string username);
    public Task<int> GetUserExp(string username);
    public Task<bool> ChangeDisplayName(string username, string newDisplayName);
}