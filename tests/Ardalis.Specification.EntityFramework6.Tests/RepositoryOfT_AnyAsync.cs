using Tests.Fixture;

namespace Tests;

[Collection("ReadCollection")]
public class RepositoryOfT_AnyAsync
{
    private readonly string _connectionString;

    public RepositoryOfT_AnyAsync(TestFactory factory)
    {
        _connectionString = factory.ConnectionString;
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
        var spec = new Specification<Store>();
        spec.Query.Where(x => x.Id == StoreSeed.VALID_STORE_ID);

        var result = await repo.AnyAsync(spec);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsFalse_GivenStoreByIdSpecWithInvalidStore()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query.Where(x => x.Id == 0);

        var result = await repo.AnyAsync(spec);

        result.Should().BeFalse();
    }
}
