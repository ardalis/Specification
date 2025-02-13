using System.Data.Entity;
using System.Threading.Tasks;

namespace Tests;

[Collection("ReadCollection")]
public class RepositoryOfT_GetBySpec
{
    private readonly string _connectionString;

    public RepositoryOfT_GetBySpec(TestFactory factory)
    {
        _connectionString = factory.ConnectionString;
    }

    [Fact]
    public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query.Where(x => x.Id == StoreSeed.VALID_STORE_ID)
            .Include(x => x.Products);

        var result = await repo.GetBySpecAsync(spec);

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Products.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task ReturnsStoreWithAddress_GivenStoreByIdIncludeAddressSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query.Where(x => x.Id == StoreSeed.VALID_STORE_ID)
            .Include(x => x.Address);

        var result = await repo.GetBySpecAsync(spec);

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
    }

    [Fact]
    public async Task ReturnsStoreWithAddressAndProduct_GivenStoreByIdIncludeAddressAndProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query.Where(x => x.Id == StoreSeed.VALID_STORE_ID)
            .Include(x => x.Address)
            .Include(x => x.Products);

        var result = await repo.GetBySpecAsync(spec);

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Products.Count.Should().BeGreaterThan(1);
        result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
    }

    [Fact]
    public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsUsingStringSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query.Where(x => x.Id == StoreSeed.VALID_STORE_ID)
            .Include(nameof(Store.Products));

        var result = await repo.GetBySpecAsync(spec);

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Products.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task ReturnsCompanyWithStoresAndAddress_GivenCompanyByIdIncludeStoresThenIncludeAddressSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Company>(dbContext);
        var spec = new Specification<Company>();
        spec.Query.Where(x => x.Id == CompanySeed.VALID_COMPANY_ID)
            .Include(x => x.Stores)
            .ThenInclude(x => x.Address);

        var result = await repo.GetBySpecAsync(spec);

        result.Should().NotBeNull();
        result.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
        result.Stores.Count.Should().BeGreaterThan(49);
        result.Stores.Select(x => x.Address).Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ReturnsCompanyWithStoresAndProducts_GivenCompanyByIdIncludeStoresThenIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Company>(dbContext);
        var spec = new Specification<Company>();
        spec.Query.Where(x => x.Id == CompanySeed.VALID_COMPANY_ID)
            .Include(x => x.Stores)
            .ThenInclude(x => x.Products);

        var result = await repo.GetBySpecAsync(spec);

        result.Should().NotBeNull();
        result.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
        result.Stores.Count.Should().BeGreaterThan(49);
        result.Stores.Select(x => x.Products).Count().Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task ReturnsUntrackedCompany_GivenCompanyByIdAsUntrackedSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Company>(dbContext);
        var spec = new Specification<Company>();
        spec.Query.Where(x => x.Id == CompanySeed.VALID_COMPANY_ID)
            .AsNoTracking();

        var result = await repo.GetBySpecAsync(spec);

        result.Should().NotBeNull();
        result?.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
        dbContext.Entry(result).State.Should().Be(EntityState.Detached);
    }

    [Fact]
    public async Task ReturnsStoreWithCompanyAndCountryAndStoresForCompany_GivenStoreByIdIncludeCompanyAndCountryAndStoresForCompanySpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query.Where(x => x.Id == StoreSeed.VALID_STORE_ID)
            .Include(x => x.Company).ThenInclude(x => x!.Country)
            .Include(x => x.Company).ThenInclude(x => x!.Stores).ThenInclude(x => x.Products);

        var result = await repo.GetBySpecAsync(spec);

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Company.Should().NotBeNull();
        result.Company?.Country.Should().NotBeNull();
        result.Company?.Stores.Should().HaveCountGreaterOrEqualTo(2);
        result.Company?.Stores?.Should().Match(x => x.Any(z => z.Products.Count > 0));
    }
}
