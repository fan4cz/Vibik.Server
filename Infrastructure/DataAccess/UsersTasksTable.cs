using Dapper;
using Infrastructure.Interfaces;
using Shared.Models;
using Npgsql;
using InterpolatedSql.SqlBuilders;
using InterpolatedSql.Dapper;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.DataAccess;

public class UsersTasksTable(NpgsqlDataSource dataSource) : IUsersTasksTable
{
    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                     SELECT
                         users_tasks.task_id               AS TaskId,
                         users_tasks.start_time::timestamp AS StartTime,
                         tasks.name                      AS Name,
                         tasks.reward                    AS Reward
                     FROM
                         users_tasks
                         JOIN tasks ON tasks.id = users_tasks.task_id
                     WHERE
                         users_tasks.username = {username} 
                         AND users_tasks.is_completed = '0'
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
                 users_tasks (
                 task_id,
                 username,
                 is_moderation_needed,
                 is_completed,
                 start_time,
                 photos_path,
                 photos_count
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
                 tasks.photos_required    AS PhotosRequired,
                 tasks.example_path       AS ExamplePhotos,
                 users_tasks.photos_path   AS UserPhotos 
             FROM
                 users_tasks
                 JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                 users_tasks.username = {username}
                 AND users_tasks.task_id = {taskId}
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
                 tasks.photos_required    AS PhotosRequired,
                 tasks.example_path       AS ExamplePhotos,
                 users_tasks.photos_path   AS UserPhotos
             FROM
                 users_tasks
                 JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                 users_tasks.id = {id}
             """);

        return (await builder.QueryAsync<TaskModelExtendedInfo>()).First();
    }

    public async Task<TaskModel?> GetTaskFullInfo(int id)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $""""
             SELECT
                users_tasks.task_id               AS TaskId,
                users_tasks.start_time::timestamp AS StartTime,
                tasks.name                      AS Name,
                tasks.reward                    AS Reward
             FROM
                users_tasks
                JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                users_tasks.id = {id}
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
                users_tasks.task_id               AS TaskId,
                users_tasks.start_time::timestamp AS StartTime,
                tasks.name                      AS Name,
                tasks.reward                    AS Reward
             FROM
                users_tasks
                JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                users_tasks.username = {username}
                AND users_tasks.task_id = {taskId}
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
                 UPDATE users_tasks
                     SET moderation_status = {moderationStatus.ToString().ToLower()}::moderation_status
                 WHERE 
                     users_tasks.username = {username}
                     AND users_tasks.task_id = {taskId}
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
                 users_tasks.task_id               AS TaskId,
                 users_tasks.start_time::timestamp AS StartTime,
                 tasks.name                      AS Name,
                 tasks.reward                    AS Reward
             From 
                 users_tasks
                 JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                 users_tasks.username = {username}
                 AND is_completed = '1'
             ORDER BY users_tasks.start_time DESC
             """);
        return (await builder.QueryAsync<TaskModel>()).ToList();
    }

    public async Task<bool> AddPhotoName(string username, string taskId, string photoName)
    {
        var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             UPDATE users_tasks
                 SET photos_path = COALESCE(photos_path, ARRAY[]::text[]) || ARRAY[{photoName}]
             WHERE 
                 users_tasks.username = {username}
                 AND users_tasks.task_id = {taskId}
             """
        );
        return await builder.ExecuteAsync() == 1;
    }
}