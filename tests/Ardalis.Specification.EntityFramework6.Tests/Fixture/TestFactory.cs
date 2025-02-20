using MartinCostello.SqlLocalDb;
using Testcontainers.MsSql;

namespace Tests.Fixture;

public class TestFactory : IAsyncLifetime
{
    // Flag to force using Docker SQL Server. Update it manually if you want to avoid localDb locally.
    private const bool _forceDocker = false;
    private MsSqlContainer? _dbContainer = null;

    public string ConnectionString { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        using (var localDB = new SqlLocalDbApi())
        {
            if (_forceDocker || !localDB.IsLocalDBInstalled())
            {
                _dbContainer = CreateContainer();
                await _dbContainer.StartAsync();
                ConnectionString = _dbContainer.GetConnectionString();
            }
            else
            {
                var databaseName = $"SpecificationEF6TestsDB_{Guid.NewGuid().ToString().Replace('-', '_')}";
                ConnectionString = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={databaseName};Integrated Security=SSPI;TrustServerCertificate=True;";
            }
        }

        using var dbContext = new TestDbContext(ConnectionString);

        dbContext.Database.CreateIfNotExists();

        dbContext.Countries.AddRange(CountrySeed.Get());
        dbContext.Companies.AddRange(CompanySeed.Get());
        dbContext.Stores.AddRange(StoreSeed.Get());
        dbContext.Addresses.AddRange(AddressSeed.Get());
        dbContext.Products.AddRange(ProductSeed.Get());

        dbContext.SaveChanges();
    }

    public async Task DisposeAsync()
    {
        if (_dbContainer is not null)
        {
            await _dbContainer.StopAsync();
        }
        else
        {
            using var dbContext = new TestDbContext(ConnectionString);
            dbContext.Database.Delete();
        }
    }

    private static MsSqlContainer CreateContainer() => new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            //.WithName("SpecificationEFCoreTestsDB")
            .WithPassword("P@ssW0rd!")
            .Build();
}
