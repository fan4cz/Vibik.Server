using Dapper;
using Infrastructure.Interfaces;
using Shared.Models;
using Npgsql;
using InterpolatedSql.SqlBuilders;
using InterpolatedSql.Dapper;

namespace Infrastructure.DataAccess;

public class UsersTasksTable(NpgsqlDataSource dataSource) : IUsersTasksTable
{
    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                     SELECT
                         userstasks.taskid               AS TaskId,
                         userstasks.starttime::timestamp AS StartTime,
                         tasks.name                      AS Name,
                         tasks.reward                    AS Reward
                     FROM
                         userstasks
                         JOIN tasks ON tasks.id = userstasks.taskid
                     WHERE
                         userstasks.username = {username} 
                         AND userstasks.iscompleted = '0'
             """);

        return (await builder.QueryAsync<TaskModel>()).ToList();
    }

    public async Task<bool> AddUserTask(string username, TaskModel taskModel)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModelExtendedInfo> GetTaskExtendedInfo(string username, string taskId)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             SELECT
                 tasks.description       AS Description,
                 tasks.photosrequired    AS PhotosRequired,
                 tasks.examplepath       AS ExamplePhotos,
                 userstasks.photospath   AS UserPhotos 
             FROM
                 userstasks
                 JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                 userstasks.username = {username}
                 AND userstasks.taskid = {taskId}
             """);

        return (await builder.QueryFirstAsync<TaskModelExtendedInfoExtension>())
            .ToTaskModelExtendedInfo();
    }

    public async Task<TaskModelExtendedInfo> GetTaskExtendedInfo(int id)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             SELECT
                 tasks.description       AS Description,
                 tasks.photosrequired    AS PhotosRequired,
                 tasks.examplepath       AS ExamplePhotos,
                 userstasks.photospath   AS UserPhotos
             FROM
                 userstasks
                 JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                 userstasks.id = {id}
             """);

        return (await builder.QueryAsync<TaskModelExtendedInfo>()).First();
    }

    public async Task<TaskModel?> GetTaskFullInfo(int id)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $""""
             SELECT
                userstasks.taskid               AS TaskId,
                userstasks.starttime::timestamp AS StartTime,
                tasks.name                      AS Name,
                tasks.reward                    AS Reward
             FROM
                userstasks
                JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                userstasks.id = {id}
             """"
        );
        var task = (await builder.QueryAsync<TaskModel>()).First();
        task.ExtendedInfo = await GetTaskExtendedInfo(id);
        return task;
    }

    public async Task<TaskModel?> GetTaskFullInfo(string username, string taskId)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $""""
             SELECT
                userstasks.taskid               AS TaskId,
                userstasks.starttime::timestamp AS StartTime,
                tasks.name                      AS Name,
                tasks.reward                    AS Reward
             FROM
                userstasks
                JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                userstasks.username = {username}
                AND userstasks.taskid = {taskId}
             """"
        );
        var task = (await builder.QueryAsync<TaskModel>()).First();
        task.ExtendedInfo = await GetTaskExtendedInfo(username, taskId);
        return task;
    }

    public async Task<bool> ChangeModerationStatus(string username, string taskId, string moderationStatus)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TaskModel>> GetUserSubmissionHistory(string username)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             SELECT
                 userstasks.taskid               AS TaskId,
                 userstasks.starttime::timestamp AS StartTime,
                 tasks.name                      AS Name,
                 tasks.reward                    AS Reward
             From 
                 userstasks
                 JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                 userstasks.username = {username}
                 AND iscompleted = '1'
             ORDER BY userstasks.starttime DESC
             """);
        return (await builder.QueryAsync<TaskModel>()).ToList();
    }
}