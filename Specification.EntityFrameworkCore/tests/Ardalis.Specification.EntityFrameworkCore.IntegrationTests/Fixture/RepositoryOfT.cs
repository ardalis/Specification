namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;

/// <inheritdoc/>
public class Repository<T>(TestDbContext dbContext,
    ISpecificationEvaluator specificationEvaluator) : RepositoryBase<T>(dbContext, specificationEvaluator), RepositoryBase<T> where T : class
{
    protected readonly TestDbContext dbContext = dbContext;

    public Repository(TestDbContext dbContext) : this(dbContext, EntityFrameworkCore.SpecificationEvaluator.Default)
    {
    }
}
