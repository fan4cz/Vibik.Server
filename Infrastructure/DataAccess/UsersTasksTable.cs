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

    public async Task<bool> AddUserTask(string username)
    {
        var taskId = await GetRandomTask();
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             INSERT INTO
                 userstasks (
                 taskid,
                 username,
                 ismoderationneeded,
                 iscompleted,
                 starttime,
                 photospath,
                 photoscount
                 )
             VALUES
                 ({taskId}, {username}, '0', '0', NOW(), NULL, 0)
             """
        );
        var rowsChanged = await builder.ExecuteAsync();
        return rowsChanged == 1;
    }

    private async Task<string> GetRandomTask()
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                     SELECT 
                         tasks.id
                     FROM tasks
                     ORDER BY random()
                     LIMIT 1;
             """
        );
        var taskId = await builder.QuerySingleAsync<string>();

        return taskId;
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

    public async Task<bool> ChangeModerationStatus(string username, string taskId, ModerationStatus moderationStatus)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                 UPDATE userstasks
                     SET moderationstatus = {moderationStatus.ToString().ToLower()}::moderationstatus
                 WHERE 
                     userstasks.username = {username}
                     AND userstasks.taskid = {taskId}
             """
        );
        return await builder.ExecuteAsync() == 1;
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

    public async Task<bool> AddPhotoName(string username, string taskId, string photoName)
    {
        var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             UPDATE userstasks
                 SET photospath = COALESCE(photospath, ARRAY[]::text[]) || ARRAY[{photoName}]
             WHERE 
                 userstasks.username = {username}
                 AND userstasks.taskid = {taskId}
             """
        );
        return await builder.ExecuteAsync() == 1;
    }
}