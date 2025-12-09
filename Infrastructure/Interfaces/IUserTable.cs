using Shared.Models;
using Shared.Models.Entities;

namespace Infrastructure.Interfaces;

public interface IUserTable
{
    public Task<User?> RegisterUser(string username, string hashPassword);
    public Task<bool> LoginUser(string username, string hashPassword);
    public Task<User?> GetUser(string username);
    public Task<bool> ChangeDisplayName(string username, string newDisplayName);
    public Task<bool> ChangeExperience(int userTaskId, int exp);
    public Task<bool> TryChangeLevel(int userTaskId);
    public Task<bool> ChangeMoney(int userTaskId, int money);
}