using Tests.Fixture;

namespace Tests;

[Collection("WriteCollection")]
public class RepositoryOfT_DeleteRangeAsync
{
    private readonly string _connectionString;

    public RepositoryOfT_DeleteRangeAsync(TestFactory factory)
    {
        _connectionString = factory.ConnectionString;
    }

    [Fact]
    public virtual async Task DeletesProductWithStoreIdOne_GivenProductByStoreIdSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);
        var spec = new Specification<Product>();
        spec.Query.Where(x => x.StoreId == 1);

        await repo.DeleteRangeAsync(spec);

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.StoreId == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);
        var spec = new Specification<Product>();
        spec.Query.Where(x => x.Id == 2);

        await repo.DeleteRangeAsync(spec);

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 2);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsUntrackedWithIdentityResolutionSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);
        var spec = new Specification<Product>();
        spec.Query
            .Where(x => x.Id == 3)
            .AsNoTrackingWithIdentityResolution();

        await repo.DeleteRangeAsync(spec);

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 3);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsTrackedSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);
        var spec = new Specification<Product>();
        spec.Query
            .Where(x => x.Id == 4)
            .AsTracking();

        await repo.DeleteRangeAsync(spec);

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 4);
    }
}
