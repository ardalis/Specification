using System.Threading.Tasks;

namespace Tests;

[Collection("ReadCollection")]
public class RepositoryOfT_GetById
{
    private readonly string _connectionString;

    public RepositoryOfT_GetById(TestFactory factory)
    {
        _connectionString = factory.ConnectionString;
    }

    [Fact]
    public async Task ReturnsStore_GivenId()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.GetByIdAsync(StoreSeed.VALID_STORE_ID);

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
    }

    [Fact]
    public async Task ReturnsStore_GivenGenericId()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.GetByIdAsync<int>(StoreSeed.VALID_STORE_ID);

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
    }
}
