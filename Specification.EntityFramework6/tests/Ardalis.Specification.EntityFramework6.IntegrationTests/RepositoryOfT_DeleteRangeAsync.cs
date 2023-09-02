using Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests;

[Collection("WriteCollection")]
public class RepositoryOfT_DeleteRangeAsync
{
    private readonly string _connectionString;

    public RepositoryOfT_DeleteRangeAsync(DatabaseFixture fixture)
    {
        _connectionString = fixture.ConnectionString;
    }

    [Fact]
    public virtual async Task DeletesProductWithStoreIdOne_GivenProductByStoreIdSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);

        await repo.DeleteRangeAsync(new ProductByStoreIdSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.StoreId == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);

        await repo.DeleteRangeAsync(new ProductByIdSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsUntrackedWithIdentityResolutionSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);

        await repo.DeleteRangeAsync(new ProductByIdAsUntrackedWithIdentityResolutionSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsUntrackedSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);

        await repo.DeleteRangeAsync(new ProductByIdAsUntrackedSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsTrackedSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);

        await repo.DeleteRangeAsync(new ProductByIdAsTrackedSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 1);
    }
}
