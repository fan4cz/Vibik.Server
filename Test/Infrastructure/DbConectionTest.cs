using Dapper;
using Npgsql;
using NUnit.Framework;
using Test;

[TestFixture]
public class DbSmokeTests : TestBase
{
    [Test]
    public async Task Db_is_available_and_schema_is_applied()
    {
        await using (var conn = await DataSource.OpenConnectionAsync())
        {
            var ping = await conn.ExecuteScalarAsync<int>("SELECT 1;");
            Assert.That(ping, Is.EqualTo(1), "PostgreSQL is not reachable (SELECT 1 failed).");
        }

        await using (var conn = await DataSource.OpenConnectionAsync())
        {
            var tables = (await conn.QueryAsync<string>("""
                                                        SELECT tablename
                                                        FROM pg_tables
                                                        WHERE schemaname = 'public'
                                                        ORDER BY tablename;
                                                        """)).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(tables, Does.Contain("tasks"), "Table 'tasks' was not created.");
                Assert.That(tables, Does.Contain("users"), "Table 'users' was not created.");
                Assert.That(tables, Does.Contain("users_tasks"), "Table 'users_tasks' was not created.");
                Assert.That(tables, Does.Contain("moderators"), "Table 'moderators' was not created.");
            });
        }
    }
}