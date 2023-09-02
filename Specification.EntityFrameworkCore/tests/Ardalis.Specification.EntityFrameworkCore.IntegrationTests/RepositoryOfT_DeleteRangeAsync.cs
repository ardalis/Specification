using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

[Collection("WriteCollection")]
public class RepositoryOfT_DeleteRangeAsync : RepositoryOfT_DeleteRangeAsync_TestKit
{
    public RepositoryOfT_DeleteRangeAsync(DatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Default)
    {
    }
}

public abstract class RepositoryOfT_DeleteRangeAsync_TestKit
{
    private readonly DbContextOptions<TestDbContext> _dbContextOptions;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected RepositoryOfT_DeleteRangeAsync_TestKit(DatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator)
    {
        _dbContextOptions = fixture.DbContextOptions;
        _specificationEvaluator = specificationEvaluator;
    }

    [Fact]
    public virtual async Task DeletesProductWithStoreIdOne_GivenProductByStoreIdSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Product>(dbContext, _specificationEvaluator);

        await repo.DeleteRangeAsync(new ProductByStoreIdSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.StoreId == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Product>(dbContext, _specificationEvaluator);

        await repo.DeleteRangeAsync(new ProductByIdSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsUntrackedWithIdentityResolutionSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Product>(dbContext, _specificationEvaluator);

        await repo.DeleteRangeAsync(new ProductByIdAsUntrackedWithIdentityResolutionSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsUntrackedSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Product>(dbContext, _specificationEvaluator);

        await repo.DeleteRangeAsync(new ProductByIdAsUntrackedSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 1);
    }

    [Fact]
    public virtual async Task DeletesProductWithIdOne_GivenProductByIdAsTrackedSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Product>(dbContext, _specificationEvaluator);

        await repo.DeleteRangeAsync(new ProductByIdAsTrackedSpec(1));

        var products = await repo.ListAsync();
        products.Should().NotBeNullOrEmpty();
        products.Should().NotContain(e => e.Id == 1);
    }
}
