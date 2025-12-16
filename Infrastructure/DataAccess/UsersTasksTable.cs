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
    IStorageService storageService) : IUsersTasksTable
{
    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                     SELECT
                         users_tasks.id                    AS UserTaskId,
                         users_tasks.task_id               AS TaskId,
                         users_tasks.start_time::timestamp AS StartTime,
                         tasks.name                      AS Name,
                         tasks.reward                    AS Reward
                     FROM
                         users_tasks
                         JOIN tasks ON tasks.id = users_tasks.task_id
                     WHERE
                         users_tasks.username = {username} 
                         AND users_tasks.moderation_status != {ModerationStatus.Approved.ToString().ToLower()}::moderation_status
             """);
        return (await builder.QueryAsync<TaskModel>()).ToList();
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
        var task = await GetTaskNoExtendedInfo(id);
        if (task is null)
            return null;
        task.ExtendedInfo = await GetTaskExtendedInfo(id);
        return task;
    }

    public async Task<TaskModel?> GetTaskNoExtendedInfo(int id)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $""""
             SELECT
                users_tasks.id                    AS UserTaskId,
                users_tasks.task_id               AS TaskId,
                users_tasks.start_time::timestamp AS StartTime,
                tasks.name                        AS Name,
                tasks.reward                      AS Reward
             FROM
                users_tasks
                JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                users_tasks.id = {id}
             """"
        );
        return await builder.QueryFirstOrDefaultAsync<TaskModel>();
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
                 users_tasks.id                    AS UserTaskId,
                 users_tasks.task_id               AS TaskId,
                 users_tasks.start_time::timestamp AS StartTime,
                 tasks.name                      AS Name,
                 tasks.reward                    AS Reward
             From 
                 users_tasks
                 JOIN tasks ON tasks.id = users_tasks.task_id
             WHERE
                 users_tasks.username = {username}
                 AND users_tasks.moderation_status = {ModerationStatus.Approved.ToString().ToLower()}::moderation_status
             ORDER BY users_tasks.start_time DESC
             """);
        return (await builder.QueryAsync<TaskModel>()).ToList();
    }

    public async Task<bool> SetPhotos(int id, string[] photosNames)
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             UPDATE users_tasks
                 SET 
                     photos_path = {photosNames}::text[],
                     photos_count = {photosNames.Length}
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

    public async Task<int> GetReward(int userTaskId)
    {
        await using var conn = await dataSource.OpenConnectionAsync();

        var builder = conn.QueryBuilder(
            $"""
             SELECT 
                tasks.reward
             FROM 
                users_tasks
                JOIN tasks ON users_tasks.task_id = tasks.id
             WHERE 
                users_tasks.id = {userTaskId}
             """
        );

        return await builder.QueryFirstOrDefaultAsync<int>();
    }
}