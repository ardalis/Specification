using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

public class RepositoryOfT_GetById : RepositoryOfT_GetById_TestKit
{
  public RepositoryOfT_GetById(SharedDatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Default)
  {
  }
}

public class RepositoryOfT_GetById_Cached : RepositoryOfT_GetById_TestKit
{
  public RepositoryOfT_GetById_Cached(SharedDatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Cached)
  {
  }
}

public abstract class RepositoryOfT_GetById_TestKit : IntegrationTestBase
{
  protected RepositoryOfT_GetById_TestKit(SharedDatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator) : base(fixture, specificationEvaluator) { }

  [Fact]
  public virtual async Task ReturnsStore_GivenId()
  {
    var result = await storeRepository.GetByIdAsync(StoreSeed.VALID_STORE_ID);

    result.Should().NotBeNull();
    result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
  }

  [Fact]
  public virtual async Task ReturnsStore_GivenGenericId()
  {
    var result = await storeRepository.GetByIdAsync<int>(StoreSeed.VALID_STORE_ID);

    result.Should().NotBeNull();
    result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
  }
}
