using Infrastructure.Interfaces;
using Shared.Models.Entities;
using Npgsql;
using InterpolatedSql.Dapper;

namespace Infrastructure.DataAccess;

public class UserTable(NpgsqlDataSource dataSource, IPasswordHasher hasher) : IUserTable
{
    public async Task<User?> RegisterUser(string username, string hashPassword)
    {
        if (await IsUsernameNotAvailable(username))
            return null;
        await using var conn = await dataSource.OpenConnectionAsync();
        var user = new User
        {
            Username = username,
            DisplayName = username.Trim(),
            Experience = 1,
            Level = 1,
            Money = 50
        };
        var builder = conn.QueryBuilder(
            $"""
                 INSERT INTO
                     users (
                     username,
                     password_hash,
                     display_name,
                     lvl,
                     exp,
                     money
                     )
                 VALUES
                     ({user.Username}, {hashPassword}, {user.DisplayName}, {user.Level}, {user.Experience}, {user.Money})
             """
        );

        var rowsChanged = await builder.ExecuteAsync();
        if (rowsChanged != 1)
            return null;
        return user;
    }

    private async Task<bool> IsUsernameNotAvailable(string username)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                 SELECT EXISTS(
                     SELECT 1 
                     FROM users 
                     WHERE username = {username}
                 );
             """
        );
        return await builder.ExecuteScalarAsync<bool>();
    }

    public async Task<bool> LoginUser(string username, string password)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                 SELECT 
                     users.password_hash
                     FROM users 
                     WHERE username = {username}
             """
        );
        var hashPassword = await builder.ExecuteScalarAsync<string>();
        return hashPassword is not null && hasher.Verify(password, hashPassword);
    }

    public async Task<User?> GetUser(string username)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                 SELECT 
                     users.username AS Username,
                     users.display_name  AS DisplayName,
                     users.exp AS Experience,
                     users.lvl AS Level,
                     users.money AS Money
                 FROM 
                     users
                 WHERE 
                     users.username = {username}
             """
        );
        return await builder.QuerySingleAsync<User?>();
    }

    public async Task<User?> GetUser(int userTaskId)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                 SELECT 
                    users.username AS Username,
                    users.display_name  AS DisplayName,
                    users.exp AS Experience,
                    users.lvl AS Level,
                    users.money AS Money
                FROM 
                    users_tasks
                    JOIN users ON users.username = users_tasks.username
                WHERE 
                    users_tasks.id = {userTaskId}
             """
        );
        return await builder.QuerySingleAsync<User?>();
    }

    public async Task<bool> ChangeDisplayName(string username, string newDisplayName)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                 UPDATE 
                     users 
                 SET
                     display_name = {newDisplayName}
                 WHERE
                     username = {username}
             """
        );
        var rowsChanged = await builder.ExecuteAsync();
        return rowsChanged == 1;
    }

    public async Task<bool> AddMoney(string username, int money)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                UPDATE
                     users
                 Set
                     money = money + {money}
                 WHERE
                     username = {username}
             """
        );

        var rowsChanged = await builder.ExecuteAsync();
        return rowsChanged == 1;
    }


    public async Task<bool> AddExperience(string username, int exp)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                UPDATE
                     users
                 Set
                     exp = exp + {exp}
                 WHERE
                     username = {username}
             """
        );
        var rowsChanged = await builder.ExecuteAsync();
        return rowsChanged == 1;
    }

    public async Task<bool> AddLevel(string username, int lvl)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                UPDATE
                     users
                 Set
                     lvl = lvl + {lvl}
                 WHERE
                     username = {username}
             """
        );
        var rowsChanged = await builder.ExecuteAsync();
        return rowsChanged == 1;
    }
}