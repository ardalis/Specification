using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using MartinCostello.SqlLocalDb;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;

public class DatabaseFixture : IDisposable
{
    public DbContextOptions<TestDbContext> DbContextOptions { get; }
    public string ConnectionString { get; }

    public DatabaseFixture()
    {
        var databaseName = $"SpecificationTestsDB_{Guid.NewGuid().ToString().Replace('-', '_')}";

        using (var localDB = new SqlLocalDbApi())
        {
            ConnectionString = localDB.IsLocalDBInstalled()
                ? $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={databaseName};Integrated Security=SSPI;MultipleActiveResultSets=True;Connection Timeout=300;Encrypt=False;TrustServerCertificate=True;"
                : $"Data Source=databaseEF;Initial Catalog={databaseName};PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!;Encrypt=False;TrustServerCertificate=True;";
        }

        Console.WriteLine($"Connection string: {ConnectionString}");

        DbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlServer(ConnectionString, x => x.EnableRetryOnFailure())
            .EnableSensitiveDataLogging()
            .Options;

        using var dbContext = new TestDbContext(DbContextOptions);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        dbContext.Countries.AddRange(CountrySeed.Get());
        dbContext.Companies.AddRange(CompanySeed.Get());
        dbContext.Stores.AddRange(StoreSeed.Get());
        dbContext.Addresses.AddRange(AddressSeed.Get());
        dbContext.Products.AddRange(ProductSeed.Get());

        dbContext.SaveChanges();
    }

    public void Dispose()
    {
        using var dbContext = new TestDbContext(DbContextOptions);
        dbContext.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
}
