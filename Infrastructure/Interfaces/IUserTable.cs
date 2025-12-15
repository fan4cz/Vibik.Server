using Shared.Models.Entities;

namespace Infrastructure.Interfaces;

public interface IUserTable
{
    public Task<User?> RegisterUser(string username, string hashPassword);
    public Task<bool> LoginUser(string username, string hashPassword);
    public Task<User?> GetUser(string username);
    public Task<User?> GetUser(int userTaskId);
    public Task<bool> ChangeDisplayName(string username, string newDisplayName);
    public Task<bool> AddExperience(string username, int exp);
    public Task<bool> AddLevel(string username, int lvl);
    public Task<bool> AddMoney(string username, int money);
}