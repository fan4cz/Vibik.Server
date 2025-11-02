using Infrastructure.DataAccess;
using Npgsql;
using Shared.Models;
using Task = Shared.Models.Task;

namespace Tests;

public class Tests
{
    private UsersTasksTable usersTasksTable = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var conectionString = "server=localhost;port=5432;database=TestDB;username=postgres;password=funPostgresSQL";
        var dataSource = NpgsqlDataSource.Create(conectionString);
        usersTasksTable = new UsersTasksTable(dataSource);
    }

    [Test]
    public async System.Threading.Tasks.Task GetListActiveUserTasksTest()
    {
        var expectedlList = new List<Task>
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