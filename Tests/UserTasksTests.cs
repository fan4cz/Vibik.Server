using Infrastructure.DataAccess;
using Npgsql;
using Shared.Models;

namespace Tests;

public class Tests
{
    private UsersTasksTable usersTasksTable = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        const string conectionString =
            "server=localhost;port=5432;database=TestDB;username=postgres;password=funPostgresSQL";
        var dataSource = NpgsqlDataSource.Create(conectionString);
        usersTasksTable = new UsersTasksTable(dataSource);
    }

    [Test]
    public async Task GetListActiveUserTasksTest()
    {
        var expectedlList = new List<TaskModel>
        {
            new() { TaskId = "Test1", Name = "Test1", StartTime = new DateTime(2025, 10, 31), Reward = 1 },
            new() { TaskId = "Test2", Name = "Test2", StartTime = new DateTime(2025, 10, 31), Reward = 2 },
            new() { TaskId = "Test3", Name = "Test3", StartTime = new DateTime(2025, 10, 31), Reward = 3 },
            new() { TaskId = "Test4", Name = "Test4", StartTime = new DateTime(2025, 10, 31), Reward = 4 }
        };


        var activeUserTasks = await usersTasksTable.GetListActiveUserTasks("TestName");
        Assert.That(
            activeUserTasks.Select(t => (t.TaskId, t.StartTime, t.Name, t.Reward)),
            Is.EquivalentTo(
                expectedlList.Select(t => (t.TaskId, t.StartTime, t.Name, t.Reward))));
    }

    [Test]
    public async Task GetTaskExtendedInfoTest()
    {
        var expected = new TaskModelExtendedInfo
        {
            Description = "Test1 Desc",
            PhotosRequired = 1,
            ExamplePhotos = ["Test1/"],
            UserPhotos = []
        };


        var activeUserTasks = await usersTasksTable.GetTaskExtendedInfo("TestName", "Test1");
        Assert.That(activeUserTasks.Description, Is.EqualTo(expected.Description));
    }

    [Test]
    public async Task GetTaskFullInfoTest()
    {
        var expected = new TaskModel
        {
            ExtendedInfo = new TaskModelExtendedInfo()
            {
                Description = "Test1 Desc",
                PhotosRequired = 1,
                ExamplePhotos =
                [
                    "Test1/"
                ],
                UserPhotos = []
            },
            TaskId = "TestName",
            Name = "Test1",
            Reward = 1,
            StartTime = new DateTime(2025, 10, 31),
        };


        var activeUserTasks = await usersTasksTable.GetTaskFullInfo("TestName", "Test1");
        Assert.That(activeUserTasks.StartTime, Is.EqualTo(expected.StartTime));
    }

    [Test]
    public async Task GetUserSubmissionHistoryTest()
    {
        var expectedlList = new List<TaskModel>
        {
            new() { TaskId = "Test1", Name = "Test1", StartTime = new DateTime(2025, 10, 31), Reward = 1 },
            new() { TaskId = "Test2", Name = "Test2", StartTime = new DateTime(2025, 10, 31), Reward = 2 },
            new() { TaskId = "Test3", Name = "Test3", StartTime = new DateTime(2025, 10, 31), Reward = 3 },
            new() { TaskId = "Test4", Name = "Test4", StartTime = new DateTime(2025, 10, 31), Reward = 4 }
        };


        var activeUserTasks = await usersTasksTable.GetListActiveUserTasks("TestName");
        Assert.That(
            activeUserTasks.Select(t => (t.TaskId, t.StartTime, t.Name, t.Reward)),
            Is.EquivalentTo(
                expectedlList.Select(t => (t.TaskId, t.StartTime, t.Name, t.Reward))));
    }
}