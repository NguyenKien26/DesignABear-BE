using DesignABear.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Xunit;

namespace DesignABear.Tests
{
    public class TestDatabaseFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("designabear_test")
            .WithUsername("test")
            .WithPassword("test")
            .Build();

        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis:7-alpine")
            .Build();

        private Respawner? _respawner;

        public string DbConnectionString => _dbContainer.GetConnectionString();
        public string RedisConnectionString => _redisContainer.GetConnectionString();

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await _redisContainer.StartAsync();

            // Run Migrations to prepare the schema
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(DbConnectionString)
                .Options;

            using var context = new AppDbContext(options);
            await context.Database.MigrateAsync();

            // Initialize Respawner for fast resets
            using var conn = new Npgsql.NpgsqlConnection(DbConnectionString);
            await conn.OpenAsync();
            _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToIgnore = new Table[] { "__EFMigrationsHistory" }
            });
        }

        public async Task ResetDatabaseAsync()
        {
            if (_respawner != null)
            {
                using var conn = new Npgsql.NpgsqlConnection(DbConnectionString);
                await conn.OpenAsync();
                await _respawner.ResetAsync(conn);
            }
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
            await _redisContainer.DisposeAsync();
        }
    }

    [CollectionDefinition("SharedContainerCollection")]
    public class SharedContainerCollection : ICollectionFixture<TestDatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
