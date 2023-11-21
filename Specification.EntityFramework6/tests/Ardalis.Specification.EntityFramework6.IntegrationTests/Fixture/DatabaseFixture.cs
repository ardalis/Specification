using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using MartinCostello.SqlLocalDb;
using System;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;

public class DatabaseFixture : IDisposable
{
    public string ConnectionString { get; }

    public DatabaseFixture()
    {
        var databaseName = $"SpecificationTestsDB_{Guid.NewGuid().ToString().Replace('-', '_')}";

        using (var localDB = new SqlLocalDbApi())
        {
            ConnectionString = localDB.IsLocalDBInstalled()
                ? $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={databaseName};Integrated Security=SSPI;MultipleActiveResultSets=True;Connection Timeout=300"
                : $"Data Source=databaseEF;Initial Catalog={databaseName};PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!;";
        }

        Console.WriteLine($"Connection string: {ConnectionString}");

        using var dbContext = new TestDbContext(ConnectionString);
        dbContext.Database.Delete();
        dbContext.Database.Create();

        dbContext.Countries.AddRange(CountrySeed.Get());
        dbContext.Companies.AddRange(CompanySeed.Get());
        dbContext.Stores.AddRange(StoreSeed.Get());
        dbContext.Addresses.AddRange(AddressSeed.Get());
        dbContext.Products.AddRange(ProductSeed.Get());

        dbContext.SaveChanges();
    }

    public void Dispose()
    {
        using var dbContext = new TestDbContext(ConnectionString);
        dbContext.Database.Delete();
        GC.SuppressFinalize(this);
    }
}
