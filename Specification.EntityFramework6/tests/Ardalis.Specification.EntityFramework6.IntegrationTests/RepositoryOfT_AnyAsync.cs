using Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests;

[Collection("ReadCollection")]
public class RepositoryOfT_AnyAsync
{
    private readonly string _connectionString;

    public RepositoryOfT_AnyAsync(DatabaseFixture fixture)
    {
        _connectionString = fixture.ConnectionString;
    }

    [Fact]
    public async Task ReturnsTrueOnStoresRecords_WithoutSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.AnyAsync();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsTrue_GivenStoreByIdSpecWithValidStore()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.AnyAsync(new StoreByIdSpec(StoreSeed.VALID_STORE_ID));

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsFalse_GivenStoreByIdSpecWithInvalidStore()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.AnyAsync(new StoreByIdSpec(0));

        result.Should().BeFalse();
    }
}
