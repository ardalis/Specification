using System;
using System.Data.Entity;
using System.Threading.Tasks;
using MartinCostello.SqlLocalDb;
using Testcontainers.MsSql;
#if NET9_0_OR_GREATER
using Respawn;
#elif NET472
using Respawn;
#endif

namespace Tests.FixtureNew;

public class TestFactory : IAsyncLifetime
{
    private const bool FORCE_DOCKER = false;
    private string _connectionString = default!;
#if NET9_0_OR_GREATER
    private Respawner _respawner = default!;
#elif NET472
    private Checkpoint _respawner = default!;
#endif
    private MsSqlContainer? _dbContainer = null;

    public string ConnectionString => _connectionString;
    public TestDbContext DbContext => new TestDbContext(_connectionString);

    public async Task ResetDatabase()
    {
#if NET9_0_OR_GREATER
        await _respawner.ResetAsync(_connectionString);
#elif NET472
        await _respawner.Reset(_connectionString);
#endif
    }

    public async Task InitializeAsync()
    {
        using (var localDB = new SqlLocalDbApi())
        {
            if (FORCE_DOCKER || !localDB.IsLocalDBInstalled())
            {
                _dbContainer = CreateContainer();
                await _dbContainer.StartAsync();
                _connectionString = _dbContainer.GetConnectionString();
            }
            else
            {
                _connectionString = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SpecificationEF6TestsDB_{Guid.NewGuid().ToString().Replace('-', '_')};Integrated Security=SSPI;TrustServerCertificate=True;";
            }
        }

        using (var dbContext = new TestDbContext(_connectionString))
        {
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
            using var dbContext = new TestDbContext(_connectionString);
            dbContext.Database.Delete();
        }
    }

    private static MsSqlContainer CreateContainer() => new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("P@ssW0rd!")
        .Build();
}
