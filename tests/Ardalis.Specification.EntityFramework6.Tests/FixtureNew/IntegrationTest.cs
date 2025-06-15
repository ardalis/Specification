using System.Collections;
using System.IO;

namespace Tests.FixtureNew;

public class IntegrationTest : IAsyncLifetime
{
    protected TestDbContext DbContext { get; private set; } = default!;
    private readonly TestFactory _testFactory;

    public IntegrationTest(TestFactory testFactory)
    {
        _testFactory = testFactory;
    }

    public static string GetQueryString<T>(TestDbContext dbContext, IQueryable<T> queryable)
    {
        // The EF6 doesn't support ToQueryString, so we need to log the SQL manually
        var writer = new StringWriter();
        dbContext.Database.Log = writer.Write;
        _ = queryable.ToList(); // Execute the query to log the SQL
        var sql = writer.ToString();

        // Remove metadata lines (connection open/close, timestamps, execution comments)
        var filteredLines = sql.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
            .Where(line =>
                !line.StartsWith("Opened connection") &&
                !line.StartsWith("Closed connection") &&
                !line.StartsWith("-- Executing") &&
                !line.StartsWith("-- Completed")
            );
        return string.Join(Environment.NewLine, filteredLines).Trim();
    }

    public Task InitializeAsync()
    {
        DbContext = new TestDbContext(_testFactory.ConnectionString);
        // On first access, there are additional queries and is skewing our tests.
        _ = DbContext.Countries.Any();
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
