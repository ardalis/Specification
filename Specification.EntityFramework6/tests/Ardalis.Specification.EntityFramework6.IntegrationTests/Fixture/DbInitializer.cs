using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using System.Data.Entity;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;

public class DbInitializer : CreateDatabaseIfNotExists<TestDbContext>
{
  protected override void Seed(TestDbContext context)
  {
    base.Seed(context);

    context.Addresses.AddRange(AddressSeed.Get());
    context.Countries.AddRange(CountrySeed.Get());
    context.Companies.AddRange(CompanySeed.Get());
    context.Products.AddRange(ProductSeed.Get());
    context.Stores.AddRange(StoreSeed.Get());

    context.SaveChanges();
  }
}
