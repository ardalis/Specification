using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

[Collection("ReadCollection")]
public class RepositoryOfT_AsAsyncEnumerable : RepositoryOfT_AnyAsync_TestKit
{
    public RepositoryOfT_AsAsyncEnumerable(DatabaseFixture fixture)
      : base(fixture, SpecificationEvaluator.Default)
    {
    }
}

[Collection("ReadCollection")]
public class RepositoryOfT_AsAsyncEnumerable_Cached : RepositoryOfT_AnyAsync_TestKit
{
    public RepositoryOfT_AsAsyncEnumerable_Cached(DatabaseFixture fixture)
      : base(fixture, SpecificationEvaluator.Cached)
    {
    }
}

public abstract class RepositoryOfT_AsAsyncEnumerable_TestKit
{
    private readonly DbContextOptions<TestDbContext> _dbContextOptions;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected RepositoryOfT_AsAsyncEnumerable_TestKit(DatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator)
    {
        _dbContextOptions = fixture.DbContextOptions;
        _specificationEvaluator = specificationEvaluator;
    }

    [Fact]
    public virtual async Task ReturnsTrueOnStoresRecords_WithoutSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var results = repo.AsAsyncEnumerable(new StoreIncludeProductsSpec());

        await foreach (var result in results.WithCancellation(CancellationToken.None))
        {
            result.Should().NotBeNull();
            result.Products.Should().NotBeEmpty();
        }
    }

    [Fact]
    public virtual async Task ReturnsStoreWithIdFrom15To30_GivenStoresByIdAsAsyncEnumerableSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var ids = Enumerable.Range(15, 16);
        var spec = new StoresByIdListSpec(ids);

        var counter = 0;
        var results = repo.AsAsyncEnumerable(spec);
        await foreach (var result in results.WithCancellation(CancellationToken.None))
        {
            result.Should().NotBeNull();
            result.Products.Should().NotBeEmpty();
            ++counter;
        }

        counter.Should().Be(16);
    }
}
