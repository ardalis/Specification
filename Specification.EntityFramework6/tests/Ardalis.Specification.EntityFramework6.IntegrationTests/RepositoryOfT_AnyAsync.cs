using Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests;

public class RepositoryOfT_AnyAsync : IntegrationTestBase
{
    public RepositoryOfT_AnyAsync(SharedDatabaseFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ReturnsTrueOnStoresRecords_WithoutSpec()
    {
        var result = await storeRepository.AnyAsync();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsTrue_GivenStoreByIdSpecWithValidStore()
    {
        var result = await storeRepository.AnyAsync(new StoreByIdSpec(StoreSeed.VALID_STORE_ID));

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsFalse_GivenStoreByIdSpecWithInvalidStore()
    {
        var result = await storeRepository.AnyAsync(new StoreByIdSpec(0));

        result.Should().BeFalse();
    }
}
