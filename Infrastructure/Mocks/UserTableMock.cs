using Infrastructure.Interfaces;
using Shared.Models;

namespace Infrastructure.Mocks;

public sealed class UserTableMock : IUserTable
{
    private readonly Dictionary<string, User> db = new(StringComparer.OrdinalIgnoreCase);

    public async Task<User?> RegisterUser(string username, string hashPassword)
    {
        if (db.ContainsKey(username)) return null;
        var user = new User { Username = username, DisplayName = username, Level = 1, Experience = 0, Money = 0 };
        db[username] = user;
        return user;
    }

    public async Task<bool> CheckPassword(string username, string hashPassword) =>
        db.ContainsKey(username); // пофиг, любой пароль пока подходит

    public async Task<User?> GetUser(string username) =>
        db.TryGetValue(username, out var u) ? u : null;

    public async Task<int> GetUserExp(string username) =>
        db.TryGetValue(username, out var u) ? u.Experience : 0;

    public async Task<bool> ChangeDisplayName(string username, string newDisplayName)
    {
        if (!db.TryGetValue(username, out var u)) return false;
        u.DisplayName = newDisplayName;
        return true;
    }
}