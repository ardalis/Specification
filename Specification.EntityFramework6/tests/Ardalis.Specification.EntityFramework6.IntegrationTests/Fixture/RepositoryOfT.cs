namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;

/// <inheritdoc/>
public class Repository<T> : RepositoryBase<T> where T : class
{
    protected readonly TestDbContext dbContext;

    public Repository(TestDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }
}
