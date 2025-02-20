namespace Tests.Fixture;

public class IntegrationTest(TestFactory testFactory) : IAsyncLifetime
{
    protected TestDbContext DbContext { get; private set; } = default!;

    public Task InitializeAsync()
    {
        DbContext = new TestDbContext(testFactory.DbContextOptions);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await testFactory.ResetDatabase();
    }

    public async Task SeedAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var dbContext = new TestDbContext(testFactory.DbContextOptions);
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task SeedRangeAsync<TEntity>(TEntity[] entities) where TEntity : class
    {
        using var dbContext = new TestDbContext(testFactory.DbContextOptions);
        dbContext.AddRange(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task SeedRangeAsync(IEnumerable<object> entities)
    {
        using var dbContext = new TestDbContext(testFactory.DbContextOptions);
        dbContext.AddRange(entities);
        await dbContext.SaveChangesAsync();
    }
}
