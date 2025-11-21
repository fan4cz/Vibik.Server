using Dapper;
using Infrastructure.Interfaces;
using Shared.Models;
using Npgsql;
using InterpolatedSql.Dapper;
using InterpolatedSql.SqlBuilders;

namespace Infrastructure.DataAccess;

public class UsersTasksTable(NpgsqlDataSource dataSource) : IUsersTasksTable
{
    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        var builder = new SqlBuilder(
            $"""
                     SELECT
                         userstasks.taskid               AS {nameof(TaskModel.TaskId)},
                         userstasks.starttime::timestamp AS {nameof(TaskModel.StartTime)},
                         tasks.name                      AS {nameof(TaskModel.Name)},
                         tasks.reward                    AS {nameof(TaskModel.Reward)}
                     FROM
                         userstasks
                         JOIN tasks ON tasks.id = userstasks.taskid
                     WHERE
                         userstasks.username = '{username}'
                         AND userstasks.iscompleted = '0'
             """);
        var query = builder.Build();
        await using var conn = await dataSource.OpenConnectionAsync();
        return (await conn.QueryAsync<TaskModel>(query.Sql, query.SqlParameters)).ToList();
    }

    public async Task<bool> AddUserTask(string username, TaskModel taskModel)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModelExtendedInfo> GetTaskExtendedInfo(string username, string taskId)
    {
        var builder = new SqlBuilder(
            $"""
             SELECT
                 tasks.description       AS {nameof(TaskModelExtendedInfo.Description)},
                 tasks.photosrequired    AS {nameof(TaskModelExtendedInfo.PhotosRequired)},
                 tasks.examplepath       AS {nameof(TaskModelExtendedInfo.ExamplePhotos)},
                 userstasks.photospath   AS {nameof(TaskModelExtendedInfo.UserPhotos)}
             FROM
                 userstasks
                 JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                 userstasks.username = '{username}'
                 AND userstasks.taskid = '{taskId}'
             """);

        var query = builder.Build();
        await using var conn = await dataSource.OpenConnectionAsync();
        return (await conn.QueryAsync<TaskModelExtendedInfo>(query.Sql, query.SqlParameters)).First();
    }

    public async Task<TaskModelExtendedInfo> GetTaskExtendedInfo(int id)
    {
        var builder = new SqlBuilder(
            $"""
             SELECT
                 tasks.description       AS {nameof(TaskModelExtendedInfo.Description)},
                 tasks.photosrequired    AS {nameof(TaskModelExtendedInfo.PhotosRequired)},
                 tasks.examplepath       AS {nameof(TaskModelExtendedInfo.ExamplePhotos)},
                 userstasks.photospath   AS {nameof(TaskModelExtendedInfo.UserPhotos)}
             FROM
                 userstasks
                 JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                 userstasks.id = {id}
             """);

        var query = builder.Build();
        await using var conn = await dataSource.OpenConnectionAsync();
        return (await conn.QueryAsync<TaskModelExtendedInfo>(query.Sql, query.SqlParameters)).First();
    }

    public async Task<TaskModel?> GetTaskFullInfo(int id)
    {
        var builder = new SqlBuilder(
            $""""
             SELECT
                userstasks.taskid               AS {nameof(TaskModel.TaskId)},
                userstasks.starttime::timestamp AS {nameof(TaskModel.StartTime)},
                tasks.name                      AS {nameof(TaskModel.Name)},
                tasks.reward                    AS {nameof(TaskModel.Reward)}
             FROM
                userstasks
                JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                userstasks.id = {id}
             """"
        );

        var query = builder.Build();
        await using var conn = await dataSource.OpenConnectionAsync();
        var task = (await conn.QueryAsync<TaskModel>(query.Sql, query.SqlParameters)).First();
        task.ExtendedInfo = await GetTaskExtendedInfo(id);
        return task;
    }

    public async Task<TaskModel?> GetTaskFullInfo(string username, string taskId)
    {
        var builder = new SqlBuilder(
            $""""
             SELECT
                userstasks.taskid               AS {nameof(TaskModel.TaskId)},
                userstasks.starttime::timestamp AS {nameof(TaskModel.StartTime)},
                tasks.name                      AS {nameof(TaskModel.Name)},
                tasks.reward                    AS {nameof(TaskModel.Reward)}
             FROM
                userstasks
                JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                userstasks.username = '{username}'
                AND userstasks.taskid = '{taskId}'
             """"
        );

        var query = builder.Build();
        await using var conn = await dataSource.OpenConnectionAsync();
        var task = (await conn.QueryAsync<TaskModel>(query.Sql, query.SqlParameters)).First();
        task.ExtendedInfo = await GetTaskExtendedInfo(username, taskId);
        return task;
    }

    public async Task<bool> ChangeModerationStatus(string username, string taskId, string moderationStatus)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TaskModel>> GetUserSubmissionHistory(string username)
    {
        var builder = new SqlBuilder(
            $"""
             SELECT
                 userstasks.taskid               AS {nameof(TaskModel.TaskId)},
                 userstasks.starttime::timestamp AS {nameof(TaskModel.StartTime)},
                 tasks.name                      AS {nameof(TaskModel.Name)},
                 tasks.reward                    AS {nameof(TaskModel.Reward)}
             From 
                 userstasks
                 JOIN tasks ON tasks.id = userstasks.taskid
             WHERE
                 userstasks.username = '{username}'
                 AND iscompleted = '1'
             ORDER BY userstasks.starttime DESC
             """);

        var query = builder.Build();
        await using var conn = await dataSource.OpenConnectionAsync();
        return (await conn.QueryAsync<TaskModel>(query.Sql, query.SqlParameters)).ToList();
    }
}