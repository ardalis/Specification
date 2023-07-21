using Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests;

public class RepositoryOfT_GetById : IntegrationTestBase
{
  public RepositoryOfT_GetById(SharedDatabaseFixture fixture) : base(fixture) { }

  [Fact]
  public async Task ReturnsStore_GivenId()
  {
    var result = await storeRepository.GetByIdAsync(StoreSeed.VALID_STORE_ID);

    result.Should().NotBeNull();
    result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
  }

  [Fact]
  public async Task ReturnsStore_GivenGenericId()
  {
    var result = await storeRepository.GetByIdAsync<int>(StoreSeed.VALID_STORE_ID);

    result.Should().NotBeNull();
    result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
  }
}
