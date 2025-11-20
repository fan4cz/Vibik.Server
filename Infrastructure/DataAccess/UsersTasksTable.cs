using Infrastructure.Interfaces;
using Shared.Models;
using Npgsql;
using Dapper;

namespace Infrastructure.DataAccess;

public class UsersTasksTable(NpgsqlDataSource dataSource) : IUsersTasksTable
{
    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        var builder = new SqlBuilder();

        var template = builder.AddTemplate("""
                                           SELECT
                                               userstasks.taskid              AS "TaskId",
                                               userstasks.starttime::timestamp AS "StartTime",
                                               tasks.name                     AS "Name",
                                               tasks.reward                   AS "Reward"
                                           FROM userstasks
                                           JOIN tasks ON tasks.id = userstasks.taskid
                                           /**where**/
                                           """);

        builder.Where("userstasks.username = @Username", new { Username = username });
        builder.Where("userstasks.iscompleted = '0'");

        await using var conn = await dataSource.OpenConnectionAsync();
        return (await conn.QueryAsync<TaskModel>(template.RawSql, template.Parameters)).ToList();
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