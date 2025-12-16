using Dapper;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using InterpolatedSql.Dapper;
using NUnit.Framework;
using Shared.Models.Enums;
using Test;

[TestFixture]
public class UsersTasksTableTests : TestBase
{
    private IUsersTasksTable table = null!;

    [SetUp]
    public void Setup()
    {
        table = Provider.GetRequiredService<IUsersTasksTable>();
    }

    [Test]
    public async Task GetTaskNoExtendedInfo_returns_task()
    {
        var id = await SeedUserTask();

        var task = await table.GetTaskNoExtendedInfo(id);

        Assert.That(task, Is.Not.Null);
        Assert.That(task!.UserTaskId, Is.EqualTo(id));
    }

    [Test]
    public async Task GetTaskExtendedInfo_returns_extended_info()
    {
        var id = await SeedUserTask(
            examplePath: ["img1.png", "img2.png"]);

        var info = await table.GetTaskExtendedInfo(id);

        Assert.That(info, Is.Not.Null);
        Assert.That(info!.ExamplePhotos, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetTaskFullInfo_returns_task_with_extension()
    {
        var id = await SeedUserTask();

        var task = await table.GetTaskFullInfo(id);

        Assert.That(task, Is.Not.Null);
        Assert.That(task!.ExtendedInfo, Is.Not.Null);
    }

    [Test]
    public async Task ChangeModerationStatus_updates_status()
    {
        var id = await SeedUserTask();

        var result = await table.ChangeModerationStatus(id, ModerationStatus.Approved);
        var status = await table.GetModerationStatus(id);

        Assert.That(result, Is.True);
        Assert.That(status, Is.EqualTo("approved"));
    }

    [Test]
    public async Task SetPhotos_replaces_array_and_count()
    {
        var id = await SeedUserTask();

        var photos = new[] { "a.png", "b.png" };
        var result = await table.SetPhotos(id, photos);

        Assert.That(result, Is.True);

        await using var conn = await DataSource.OpenConnectionAsync();

        var count = await conn.QueryBuilder(
            $"""
             SELECT photos_count
             FROM users_tasks
             WHERE id = {id}
             """
        ).ExecuteScalarAsync<int>();

        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetModerationTask_returns_waiting_task()
    {
        await SeedUserTask(moderationStatus: "waiting");

        var task = await table.GetModerationTask();

        Assert.That(task, Is.Not.Null);
        Assert.That(task!.ExtendedInfo, Is.Not.Null);
    }

    [Test]
    public async Task GetReward_returns_reward()
    {
        var id = await SeedUserTask();

        var reward = await table.GetReward(id);

        Assert.That(reward, Is.EqualTo(10));
    }

    private async Task<int> SeedUserTask(
        string username = "user",
        string taskId = "task1",
        string moderationStatus = "default",
        string[]? examplePath = null)
    {
        await using var conn = await DataSource.OpenConnectionAsync();

        await conn.QueryBuilder(
            $"""
             INSERT INTO users (username)
             VALUES ({username})
             """
        ).ExecuteAsync();

        await conn.QueryBuilder(
            $"""
             INSERT INTO tasks (id, name, reward, example_path)
             VALUES ({taskId}, 'Task', 10, {examplePath}::text[])
             """
        ).ExecuteAsync();

        var builder = conn.QueryBuilder(
            $"""
             INSERT INTO users_tasks (task_id, username, moderation_status)
             VALUES (
                {taskId},
                {username},
                {moderationStatus}::moderation_status
             )
             RETURNING id
             """
        );

        return await builder.ExecuteScalarAsync<int>();
    }
}