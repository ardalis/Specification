using Ardalis.Specification.UnitTests.Fixture.Entities;
using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;

public class IntegrationTestBase : IClassFixture<SharedDatabaseFixture>
{
  protected TestDbContext dbContext;
  protected Repository<Company> companyRepository;
  protected Repository<Store> storeRepository;

  public IntegrationTestBase(SharedDatabaseFixture fixture)
  {
    dbContext = fixture.CreateContext();

    companyRepository = new Repository<Company>(dbContext);
    storeRepository = new Repository<Store>(dbContext);
  }
}
