using Infrastructure.Interfaces;
using Shared.Models;
using Npgsql;
using Dapper;

namespace Infrastructure.DataAccess;

public class UsersTasksTable(NpgsqlDataSource dataSource) : IUsersTasksTable
{
    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        const string sql = """

                           SELECT
                               userstasks.taskid,
                               userstasks.starttime::timestamp,
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
        return (await conn.QueryAsync<TaskModel>(sql, new { username })).ToList();
    }

    public async Task<bool> AddUserTask(string username, TaskModel taskModel)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModelExtendedInfo> GetTaskExtendedInfo(string username, string taskId)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModelExtendedInfo> GetTaskExtendedInfo(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModel?> GetTaskFullInfo(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModel?> GetTaskFullInfo(string taskId, string username)
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