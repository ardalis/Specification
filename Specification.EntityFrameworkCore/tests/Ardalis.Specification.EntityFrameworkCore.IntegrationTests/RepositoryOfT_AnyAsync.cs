using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests
{
  public class RepositoryOfT_AnyAsync : RepositoryOfT_AnyAsync_TestKit
  {
    public RepositoryOfT_AnyAsync(SharedDatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Default)
    {
    }
  }

  public class RepositoryOfT_AnyAsync_Cached : RepositoryOfT_AnyAsync_TestKit
  {
    public RepositoryOfT_AnyAsync_Cached(SharedDatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Cached)
    {
    }
  }

  public abstract class RepositoryOfT_AnyAsync_TestKit : IntegrationTestBase
  {
    protected RepositoryOfT_AnyAsync_TestKit(SharedDatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator) : base(fixture, specificationEvaluator) { }

    [Fact]
    public virtual async Task ReturnsTrueOnStoresRecords_WithoutSpec()
    {
      var result = await storeRepository.AnyAsync();

      result.Should().BeTrue();
    }

    [Fact]
    public virtual async Task ReturnsTrue_GivenStoreByIdSpecWithValidStore()
    {
      var result = await storeRepository.AnyAsync(new StoreByIdSpec(StoreSeed.VALID_STORE_ID));

      result.Should().BeTrue();
    }

    [Fact]
    public virtual async Task ReturnsTrue_GivenStoreByIdSpecWithInvalidStore()
    {
      var result = await storeRepository.AnyAsync(new StoreByIdSpec(0));

      result.Should().BeFalse();
    }
  }
}
