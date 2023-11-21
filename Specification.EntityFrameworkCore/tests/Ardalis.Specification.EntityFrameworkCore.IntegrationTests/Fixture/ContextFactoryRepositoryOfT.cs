using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;

public class ContextFactoryRepository<T, TContext> : ContextFactoryRepositoryBaseOfT<T, TContext>
  where T : class where TContext : DbContext
{
    public ContextFactoryRepository(IDbContextFactory<TContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    public ContextFactoryRepository(IDbContextFactory<TContext> dbContextFactory, ISpecificationEvaluator specificationEvaluator)
        : base(dbContextFactory, specificationEvaluator)
    {
    }
}
