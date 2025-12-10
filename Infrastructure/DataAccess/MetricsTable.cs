using Infrastructure.Interfaces;
using InterpolatedSql.Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.DataAccess;

public class MetricsTable(NpgsqlDataSource dataSource, ILogger<UsersTasksTable> logger) : IMetricsTable
{
    public async Task<bool> AddRecord(string username, MetricType type)
    {
        logger.LogInformation("вызов AddRecord для username: {username}, type: {type}", username, type);
        Console.WriteLine($"вызов AddRecord для username: {username}, type: {type}");
        await using var conn = await dataSource.OpenConnectionAsync();
        var time = DateTime.UtcNow;
        var builder = conn.QueryBuilder(
            $"""
                     INSERT INTO
                         metrics (username, type, time)
                     VALUES
                         ({username}, {type}, {time})
             """);
        var rows = await builder.ExecuteAsync();
        return rows == 1;
    }

    public async Task<List<MetricModel>> ReadAllRecord()
    {
        logger.LogInformation("вызов ReadAllRecord");
        Console.WriteLine("вызов ReadAllRecord");
        await using var conn = await dataSource.OpenConnectionAsync();
        var builder = conn.QueryBuilder(
            $"""
                     SELECT
                         id       AS Id,
                         username AS Username,
                         type     AS Type,
                         time     AS Time
                     FROM
                         metrics
             """);
        return (await builder.QueryAsync<MetricModel>()).ToList();
    }
}