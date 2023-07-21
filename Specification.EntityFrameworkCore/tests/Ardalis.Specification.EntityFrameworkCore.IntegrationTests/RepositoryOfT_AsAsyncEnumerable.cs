using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

public class RepositoryOfT_AsAsyncEnumerable : RepositoryOfT_AnyAsync_TestKit
{
  public RepositoryOfT_AsAsyncEnumerable(SharedDatabaseFixture fixture)
    : base(fixture, SpecificationEvaluator.Default)
  {
  }
}

public abstract class RepositoryOfT_AsAsyncEnumerable_TestKit : IntegrationTestBase
{
  protected RepositoryOfT_AsAsyncEnumerable_TestKit(SharedDatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator)
    : base(fixture, specificationEvaluator)
  {
  }

  [Fact]
  public virtual async Task ReturnsTrueOnStoresRecords_WithoutSpec()
  {
    var results = storeRepository.AsAsyncEnumerable(new StoreIncludeProductsSpec());

    await foreach (var result in results.WithCancellation(CancellationToken.None))
    {
      result.Should().NotBeNull();
      result.Products.Should().NotBeEmpty();
    }
  }

  [Fact]
  public virtual async Task ReturnsStoreWithIdFrom15To30_GivenStoresByIdAsAsyncEnumerableSpec()
  {
    var ids = Enumerable.Range(15, 16);
    var spec = new StoresByIdListSpec(ids);

    var counter = 0;
    var results = storeRepository.AsAsyncEnumerable(spec);
    await foreach (var result in results.WithCancellation(CancellationToken.None))
    {
      result.Should().NotBeNull();
      result.Products.Should().NotBeEmpty();
      ++counter;
    }

    counter.Should().Be(16);
  }
}
