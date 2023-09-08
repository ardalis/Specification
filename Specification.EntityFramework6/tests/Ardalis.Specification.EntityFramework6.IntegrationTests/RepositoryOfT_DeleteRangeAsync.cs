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

        await repo.DeleteRangeAsync(new ProductByIdSpec(2));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 2);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsUntrackedWithIdentityResolutionSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);

        await repo.DeleteRangeAsync(new ProductByIdAsUntrackedWithIdentityResolutionSpec(3));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 3);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsTrackedSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Product>(dbContext, SpecificationEvaluator.Default);

        await repo.DeleteRangeAsync(new ProductByIdAsTrackedSpec(4));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 4);
    }
}
