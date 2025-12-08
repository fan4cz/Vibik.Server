using Infrastructure.DbExtensions;
using Infrastructure.Interfaces;
using Npgsql;
using InterpolatedSql.Dapper;
using Shared.Models.Entities;
using Shared.Models.Enums;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataAccess;

public class UsersTasksTable(
    NpgsqlDataSource dataSource,
    ILogger<UsersTasksTable> logger,
    IStorageService storageService) : IUsersTasksTable
{
    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        logger.LogInformation("вызов GetListActiveUserTasks для username: {username} ", username);
        Console.WriteLine($"вызов GetListActiveUserTasks для username: {username}");
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

    public async Task<TaskModel?> AddUserTask(string username)
    {
        var task = await GetRandomTask();
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             INSERT INTO
                 users_tasks (
                 task_id,
                 username,
                 moderation_status,
                 is_completed,
                 start_time,
                 photos_path,
                 photos_count
                 )
             VALUES
                 ({task.TaskId}, {username}, {ModerationStatus.Default.ToString().ToLower()}::moderation_status, '0', NOW(), NULL, 0)
             """
        );
        var rowsChanged = await builder.ExecuteAsync();
        return rowsChanged == 1 ? task : null;
    }

    public async Task<TaskModel?> ChangeUserTask(string username, string taskId)
    {
        var task = await GetRandomTask();

        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             UPDATE users_tasks
             SET
                 task_id = {task.TaskId},
                 moderation_status = {ModerationStatus.Default.ToString().ToLower()}::moderation_status,
                 is_completed = '0',
                 start_time = NOW(),
                 photos_path = NULL,
                 photos_count = 0
             WHERE
                 username = {username}
                 AND task_id = {taskId}
             """
        );

        var rowsChanged = await builder.ExecuteAsync();
        return rowsChanged == 1 ? task : null;
    }

    public async Task<TaskModel?> ChangeUserTask(int id)
    {
        var task = await GetRandomTask();

        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             UPDATE users_tasks
             SET
                 task_id = {task.TaskId},
                 moderation_status = {ModerationStatus.Default.ToString().ToLower()}::moderation_status,
                 is_completed = '0',
                 start_time = NOW(),
                 photos_path = NULL,
                 photos_count = 0
             WHERE
                 id = {id}
             """
        );

        var rowsChanged = await builder.ExecuteAsync();
        return rowsChanged == 1 ? task : null;
    }

    private async Task<TaskModel> GetRandomTask()
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             SELECT
                 tasks.id              AS TaskId,
                 now()::timestamp      AS StartTime,
                 tasks.name            AS Name,
                 tasks.reward          AS Reward
             FROM
                 tasks
             ORDER BY random()
             LIMIT 1;
             """
        );
        var taskId = await builder.QuerySingleAsync<TaskModel>();

        return taskId;
    }

    public async Task<TaskModelExtendedInfo?> GetTaskExtendedInfo(string username, string taskId)
    {
        logger.LogInformation("вызов GetTaskExtendedInfo для username: {username} task: {taskId} ", username, taskId);
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             SELECT
                 tasks.description                        AS Description,
                 tasks.photos_required                    AS PhotosRequired,
                 COALESCE(tasks.example_path, ARRAY[]::text[]) AS ExamplePhotos,
                 COALESCE(users_tasks.photos_path, ARRAY[]::text[]) AS UserPhotos 
             FROM
                 users_tasks
                 JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                 users_tasks.username = {username}
                 AND users_tasks.task_id = {taskId}
             """);
        var result = await builder.QueryFirstOrDefaultAsync<TaskModelExtendedInfoDbExtension>();
        if (result is null)
            return null;
        return await result.ToTaskModelExtendedInfo(storageService);
    }

    public async Task<TaskModelExtendedInfo?> GetTaskExtendedInfo(int id)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             SELECT
                 tasks.description                        AS Description,
                 tasks.photos_required                    AS PhotosRequired,
                 COALESCE(tasks.example_path, ARRAY[]::text[]) AS ExamplePhotos,
                 COALESCE(users_tasks.photos_path, ARRAY[]::text[]) AS UserPhotos
             FROM
                 users_tasks
                 JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                 users_tasks.id = {id}
             """);

        var result = await builder.QueryFirstOrDefaultAsync<TaskModelExtendedInfoDbExtension>();
        if (result is null)
            return null;
        return await result.ToTaskModelExtendedInfo(storageService);
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
        logger.LogInformation("вызов GetTaskFullInfo для username: {username} task: {taskId} ", username, taskId);
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
        var task = await builder.QueryFirstOrDefaultAsync<TaskModel>();
        if (task is null)
            return null;
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

    public async Task<bool> ChangeModerationStatus(int id, ModerationStatus moderationStatus)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                 UPDATE users_tasks
                     SET moderation_status = {moderationStatus.ToString().ToLower()}::moderation_status
                 WHERE 
                    users_tasks.id = {id}
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

    public async Task<bool> AddPhoto(string username, string taskId, string photoName)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             UPDATE users_tasks
                 SET 
                     photos_path = COALESCE(photos_path, ARRAY[]::text[]) || ARRAY[{photoName}],
                     photos_count = photos_count + 1
             WHERE 
                 users_tasks.username = {username}
                 AND users_tasks.task_id = {taskId}
             """
        );
        return await builder.ExecuteAsync() == 1;
    }

    public async Task<bool> AddPhoto(int id, string photoName)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             UPDATE users_tasks
                 SET 
                     photos_path = COALESCE(photos_path, ARRAY[]::text[]) || ARRAY[{photoName}],
                     photos_count = photos_count + 1
             WHERE 
                 users_tasks.id = {id}
             """
        );
        return await builder.ExecuteAsync() == 1;
    }

    public async Task<ModerationTask?> GetModerationTask()
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $""""
                 SELECT
                     users_tasks.id                  AS UserTaskId,
                     users_tasks.task_id             AS TaskId,    
                     tasks.name                      AS Name,
                     tasks.reward                    AS Reward, 
                     tasks.tags                      AS Tags
                 FROM
                     users_tasks
                 JOIN tasks ON tasks.id = users_tasks.task_id
                 WHERE
                     users_tasks.moderation_status = {ModerationStatus.Waiting.ToString().ToLower()}::moderation_status
                 ORDER BY users_tasks.id
             """"
        );
        var taskExtension = await builder.QueryFirstOrDefaultAsync<ModerationTaskDbExtension>();
        if (taskExtension is null)
            return null;
        var task = await taskExtension.ToModerationTask();
        task.ExtendedInfo = await GetTaskExtendedInfo(task.UserTaskId);
        return task;
    }
    
    public async Task<string> GetModerationStatus(int id)
    {
        await using var conn = await dataSource.OpenConnectionAsync();

        var builder = conn.QueryBuilder(
            $"""
             SELECT 
                 moderation_status 
             FROM 
                 users_tasks
             WHERE 
                 id = {id}
             """
        );
        
        return await builder.QueryFirstOrDefaultAsync<string>();
    }

}