using Infrastructure.Interfaces;
using Shared.Models;
using Task = Shared.Models.Task;
using Npgsql;
using Dapper;

namespace Infrastructure.DataAccess;

public class UsersTasksTable(NpgsqlDataSource dataSource) : IUsersTasksTable
{
    private readonly NpgsqlDataSource dataSourse = dataSource;

    public async Task<List<Task>> GetListActiveUserTasks(string username)
    {
        const string sql = """

                           SELECT
                               userstasks.taskid,
                               userstasks.starttime,
                               tasks.name,
                               tasks.reward
                           FROM
                               userstasks
                               JOIN tasks ON tasks.id = userstasks.taskid
                           WHERE
                               userstasks.username = @Username
                               AND userstasks.iscompleted = '0'

                           """;
        await using var conn = await dataSource.OpenConnectionAsync();
        return (await conn.QueryAsync<Task>(sql, new { username })).ToList();
    }

    public async Task<bool> AddUserTask(string username, Task task)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskExtendedInfo> GetTaskExtendedInfo(string username, string taskId)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskExtendedInfo> GetTaskExtendedInfo(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Task?> GetTaskFullInfo(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Task?> GetTaskFullInfo(string taskId, string username)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ChangeModerationStatus(string username, string taskId, string moderationStatus)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }
}