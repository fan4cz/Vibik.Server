using Dapper;
using DotNetEnv;
using Infrastructure.DataAccess;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Test.Fake;

namespace Test;

public abstract class TestBase
{
    protected ServiceProvider Provider = null!;
    protected NpgsqlDataSource DataSource = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Env.TraversePath().Load();

        var cs = BuildConnectionString();

        var services = new ServiceCollection();
        services.AddSingleton(NpgsqlDataSource.Create(cs));

        services.AddScoped<IUsersTasksTable, UsersTasksTable>();
        services.AddScoped<IUserTable, UserTable>();
        services.AddScoped<ITaskEvent, RandomTaskEvent>();
        services.AddSingleton<IStorageService, FakeStorageService>();
        services.AddSingleton<IPasswordHasher, FakePasswordHasher>();

        Provider = services.BuildServiceProvider();
        DataSource = Provider.GetRequiredService<NpgsqlDataSource>();

        await InitSchema();
    }

    private static string BuildConnectionString()
    {
        string Get(string name) =>
            Environment.GetEnvironmentVariable(name)
            ?? throw new InvalidOperationException($"{name} is missing");

        return
            $"Host={Get("POSTGRES_SERVER")};" +
            $"Port={Get("POSTGRES_PORT")};" +
            $"Database={Get("POSTGRES_DB")};" +
            $"Username={Get("POSTGRES_USER")};" +
            $"Password={Get("POSTGRES_PASSWORD")};";
    }

    private async Task InitSchema()
    {
        await using var conn = await DataSource.OpenConnectionAsync();

        var sql = await File.ReadAllTextAsync(
            Path.Combine("db_init", "001_schema.sql"));

        await conn.ExecuteAsync(sql);
    }

    [SetUp]
    public async Task CleanDb()
    {
        await using var conn = await DataSource.OpenConnectionAsync();

        await conn.ExecuteAsync("""
                                TRUNCATE TABLE
                                    users_tasks,
                                    tasks,
                                    users
                                RESTART IDENTITY
                                CASCADE;
                                """);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await DataSource.DisposeAsync();
        await Provider.DisposeAsync();
    }
}