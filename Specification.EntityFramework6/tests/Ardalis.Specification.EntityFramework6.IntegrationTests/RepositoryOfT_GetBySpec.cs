using Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests;

[Collection("ReadCollection")]
public class RepositoryOfT_GetBySpec
{
    private readonly string _connectionString;

    public RepositoryOfT_GetBySpec(DatabaseFixture fixture)
    {
        _connectionString = fixture.ConnectionString;
    }

    [Fact]
    public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeProductsSpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Products.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task ReturnsStoreWithAddress_GivenStoreByIdIncludeAddressSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeAddressSpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
    }

    [Fact]
    public async Task ReturnsStoreWithAddressAndProduct_GivenStoreByIdIncludeAddressAndProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeAddressAndProductsSpec(StoreSeed.VALID_STORE_ID));

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

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeProductsUsingStringSpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Products.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task ReturnsCompanyWithStoresAndAddress_GivenCompanyByIdIncludeStoresThenIncludeAddressSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Company>(dbContext);

        var result = await repo.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeAddressSpec(CompanySeed.VALID_COMPANY_ID));

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

        var result = await repo.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeProductsSpec(CompanySeed.VALID_COMPANY_ID));

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

        var result = await repo.GetBySpecAsync(new CompanyByIdAsUntrackedSpec(CompanySeed.VALID_COMPANY_ID));

        result.Should().NotBeNull();
        result?.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
        dbContext.Entry(result).State.Should().Be(EntityState.Detached);
    }

    [Fact]
    public async Task ReturnsStoreWithCompanyAndCountryAndStoresForCompany_GivenStoreByIdIncludeCompanyAndCountryAndStoresForCompanySpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeCompanyAndCountryAndStoresForCompanySpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Company.Should().NotBeNull();
        result.Company?.Country.Should().NotBeNull();
        result.Company?.Stores.Should().HaveCountGreaterOrEqualTo(2);
        result.Company?.Stores?.Should().Match(x => x.Any(z => z.Products.Count > 0));
    }
}
