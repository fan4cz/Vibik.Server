using Dapper;
using Infrastructure.Interfaces;
using InterpolatedSql.Dapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Models.Entities;
using Test;

[TestFixture]
public class UserTableTests : TestBase
{
    private IUserTable users = null!;

    [SetUp]
    public void Setup()
    {
        users = Provider.GetRequiredService<IUserTable>();
    }

    [Test]
    public async Task RegisterUser_creates_user()
    {
        var user = await users.RegisterUser("test", "HASH::123");

        Assert.That(user, Is.Not.Null);
        Assert.That(user!.Username, Is.EqualTo("test"));
        Assert.That(user.Level, Is.EqualTo(1));
        Assert.That(user.Money, Is.EqualTo(50));
    }

    [Test]
    public async Task RegisterUser_returns_null_if_username_exists()
    {
        await users.RegisterUser("test", "HASH::123");

        var second = await users.RegisterUser("test", "HASH::456");

        Assert.That(second, Is.Null);
    }

    [Test]
    public async Task LoginUser_returns_true_for_correct_password()
    {
        await users.RegisterUser("test", "HASH::123");

        var result = await users.LoginUser("test", "123");

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task LoginUser_returns_false_for_wrong_password()
    {
        await users.RegisterUser("test", "HASH::123");

        var result = await users.LoginUser("test", "wrong");

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetUser_by_username_returns_user()
    {
        await users.RegisterUser("test", "HASH::123");

        var user = await users.GetUser("test");

        Assert.That(user, Is.Not.Null);
        Assert.That(user!.Username, Is.EqualTo("test"));
    }

    [Test]
    public async Task ChangeDisplayName_updates_name()
    {
        await users.RegisterUser("test", "HASH::123");

        var changed = await users.ChangeDisplayName("test", "NewName");
        var user = await users.GetUser("test");

        Assert.That(changed, Is.True);
        Assert.That(user!.DisplayName, Is.EqualTo("NewName"));
    }

    [Test]
    public async Task AddMoney_increases_money()
    {
        await users.RegisterUser("test", "HASH::123");

        await users.AddMoney("test", 25);
        var user = await users.GetUser("test");

        Assert.That(user!.Money, Is.EqualTo(75));
    }

    [Test]
    public async Task AddExperience_increases_exp()
    {
        await users.RegisterUser("test", "HASH::123");

        await users.AddExperience("test", 10);
        var user = await users.GetUser("test");

        Assert.That(user!.Experience, Is.EqualTo(11));
    }

    [Test]
    public async Task AddLevel_increases_level()
    {
        await users.RegisterUser("test", "HASH::123");

        await users.AddLevel("test", 2);
        var user = await users.GetUser("test");

        Assert.That(user!.Level, Is.EqualTo(3));
    }

    [Test]
    public async Task GetUser_by_userTaskId_returns_user()
    {
        await users.RegisterUser("test", "HASH::123");

        await using var conn = await DataSource.OpenConnectionAsync();

        var taskId = await conn.QueryBuilder(
            $"""
             INSERT INTO users_tasks (task_id, username)
             VALUES ('t1', 'test')
             RETURNING id;
             """
        ).ExecuteScalarAsync<int>();

        var user = await users.GetUser(taskId);

        Assert.That(user, Is.Not.Null);
        Assert.That(user!.Username, Is.EqualTo("test"));
    }
}