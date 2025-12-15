using Infrastructure.Interfaces;
using Npgsql;
using Shared.Models.Entities;
using InterpolatedSql.Dapper;
using Shared.Models.Enums;

namespace Infrastructure.DataAccess;

public class RandomTaskEvent(NpgsqlDataSource dataSource) : ITaskEvent
{
    public async Task<TaskModel> AddUserTask(string username)
    {
        var taskId = await GetRandomTask();
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             INSERT INTO
                 users_tasks (
                 task_id,
                 username,
                 moderation_status,
                 start_time,
                 photos_path,
                 photos_count
                 )
             VALUES
                 ({taskId}, {username}, {ModerationStatus.Default.ToString().ToLower()}::moderation_status, NOW(), NULL, 0)
             RETURNING
                 users_tasks.id AS UserTaskId,
                 users_tasks.task_id AS TaskId,
                 users_tasks.start_time::timestamp  AS StartTime,
                 (SELECT name FROM tasks WHERE id = users_tasks.task_id) AS Name,
                 (SELECT reward FROM tasks WHERE id = users_tasks.task_id) AS Reward
             """
        );
        var task = await builder.QueryFirstAsync<TaskModel>();
        return task;
    }

    public async Task<TaskModel?> ChangeUserTask(int id)
    {
        var taskId = await GetRandomTask();

        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
             UPDATE 
                 users_tasks
             SET
                 task_id = {taskId},
                 moderation_status = {ModerationStatus.Default.ToString().ToLower()}::moderation_status,
                 start_time = NOW(),
                 photos_path = NULL,
                 photos_count = 0
             WHERE
                 id = {id}
             RETURNING
                 users_tasks.id AS UserTaskId,
                 users_tasks.task_id AS TaskId,
                 users_tasks.start_time::timestamp  AS StartTime,
                 (SELECT name FROM tasks WHERE id = users_tasks.task_id) AS Name,
                 (SELECT reward FROM tasks WHERE id = users_tasks.task_id) AS Reward
             """
        );

        var task = await builder.QueryFirstAsync<TaskModel>();
        return task;
    }

    private async Task<string> GetRandomTask()
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                     SELECT
                         id
                     FROM
                         tasks
                     ORDER BY random()
                        LIMIT 1;
             """
        );
        var taskId = await builder.QuerySingleAsync<string>();

        return taskId;
    }
}