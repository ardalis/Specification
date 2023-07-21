using Ardalis.Specification.UnitTests.Fixture.Entities;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;

public abstract class IntegrationTestBase : IClassFixture<SharedDatabaseFixture>
{
    protected TestDbContext dbContext;
    protected Repository<Company> companyRepository;
    protected Repository<Store> storeRepository;

    protected IntegrationTestBase(SharedDatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator)
    {
        dbContext = fixture.CreateContext();

        companyRepository = new Repository<Company>(dbContext, specificationEvaluator);
        storeRepository = new Repository<Store>(dbContext, specificationEvaluator);
    }
}
