using MartinCostello.SqlLocalDb;
using Respawn;
using Testcontainers.MsSql;

namespace Tests.Fixture;

public class TestFactory : IAsyncLifetime
{
    // Flag to force using Docker SQL Server. Update it manually if you want to avoid localDb locally.
    private const bool FORCE_DOCKER = false;

    private string _connectionString = default!;
    private MsSqlContainer? _dbContainer = null;

    public string ConnectionString => _connectionString;
    public TestDbContext DbContext => new TestDbContext(_connectionString);

#if NET9_0_OR_GREATER
    private Respawner _respawner = default!;
    public async Task ResetDatabase() => await _respawner.ResetAsync(_connectionString);
#elif NET472
    private Checkpoint _respawner = default!;
    public Task ResetDatabase() => _respawner.Reset(_connectionString);
#endif

    public async Task InitializeAsync()
    {
#if NET9_0_OR_GREATER
        var dbName = "SpecificationTestDB_EF6_NET9";
#elif NET472
        var dbName = "SpecificationTestDB_EF6_NETFFX";
#endif

        using (var localDB = new SqlLocalDbApi())
        {
            if (FORCE_DOCKER || !localDB.IsLocalDBInstalled())
            {
                _dbContainer = CreateContainer();
                await _dbContainer.StartAsync();
                var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(_dbContainer.GetConnectionString())
                {
                    InitialCatalog = dbName
                };
                _connectionString = builder.ToString();
            }
            else
            {
                _connectionString = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=SSPI;TrustServerCertificate=True;";
            }
        }

        Console.WriteLine($"Connection string: {_connectionString}");

        using (var dbContext = new TestDbContext(_connectionString))
        {
            //dbContext.Database.Delete();
            dbContext.Database.CreateIfNotExists();
        }

#if NET9_0_OR_GREATER
        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" },
        });
        await ResetDatabase();
#elif NET472
        _respawner = new Checkpoint
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" },
        };
        await ResetDatabase();
#endif
    }

    public async Task DisposeAsync()
    {
        if (_dbContainer is not null)
        {
            await _dbContainer.StopAsync();
        }
        else
        {
            //using var dbContext = new TestDbContext(_connectionString);
            //dbContext.Database.Delete();
        }
    }

    private static MsSqlContainer CreateContainer() => new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("P@ssW0rd!")
            .Build();
}
