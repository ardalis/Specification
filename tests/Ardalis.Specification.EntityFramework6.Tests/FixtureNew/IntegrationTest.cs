using System.Collections;

namespace Tests.FixtureNew;

public class IntegrationTest : IAsyncLifetime
{
    protected TestDbContext DbContext { get; private set; } = default!;
    private readonly TestFactory _testFactory;

    public IntegrationTest(TestFactory testFactory)
    {
        _testFactory = testFactory;
    }

    public Task InitializeAsync()
    {
        DbContext = new TestDbContext(_testFactory.ConnectionString);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        DbContext.Dispose();
        await _testFactory.ResetDatabase();
    }

    public async Task SeedAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var dbContext = new TestDbContext(_testFactory.ConnectionString);
        dbContext.Set<TEntity>().Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task SeedRangeAsync<TEntity>(TEntity[] entities) where TEntity : class
    {
        using var dbContext = new TestDbContext(_testFactory.ConnectionString);
        dbContext.Set<TEntity>().AddRange(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task SeedRangeAsync(IEnumerable entities)
    {
        using var dbContext = new TestDbContext(_testFactory.ConnectionString);
        foreach (var entity in entities)
        {
            dbContext.Set(entity.GetType()).Add(entity);
        }
        await dbContext.SaveChangesAsync();
    }
}
