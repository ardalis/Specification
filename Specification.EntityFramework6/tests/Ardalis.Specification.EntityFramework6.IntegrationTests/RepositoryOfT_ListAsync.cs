using Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests;

[Collection("ReadCollection")]
public class RepositoryOfT_ListAsync
{
    private readonly string _connectionString;

    public RepositoryOfT_ListAsync(DatabaseFixture fixture)
    {
        _connectionString = fixture.ConnectionString;
    }

    [Fact]
    public async Task ReturnsStoreWithProducts_GivenStoreIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.ListAsync(new StoreIncludeProductsSpec());

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Products.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ReturnsStoreWithAddress_GivenStoreIncludeAddressSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.ListAsync(new StoreIncludeAddressSpec());

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Address.Should().NotBeNull();
    }

    [Fact]
    public async Task ReturnsStoreWithAddressAndProduct_GivenStoreIncludeAddressAndProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.ListAsync(new StoreIncludeAddressAndProductsSpec());

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Address.Should().NotBeNull();
        result[0].Products.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ReturnsStoreWithIdFrom15To30_GivenStoresByIdListSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var ids = Enumerable.Range(15, 16);
        var spec = new StoresByIdListSpec(ids);

        var stores = await repo.ListAsync(spec);

        stores.Count.Should().Be(16);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(15);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(30);
    }

    [Fact]
    public async Task ReturnsSecondPageOfStoreNames_GivenStoreNamesPaginatedSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var take = 10; // pagesize 10
        var skip = (2 - 1) * 10; // page 2

        var spec = new StoreNamesPaginatedSpec(skip, take);

        var storeNames = await repo.ListAsync(spec);

        storeNames.Count.Should().Be(take);
        storeNames.First().Should().Be("Store 11");
        storeNames.Last().Should().Be("Store 20");
    }

    [Fact]
    public async Task ReturnsSecondPageOfStores_GivenStoresPaginatedSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var take = 10; // pagesize 10
        var skip = (2 - 1) * 10; // page 2

        var spec = new StoresPaginatedSpec(skip, take);

        var stores = await repo.ListAsync(spec);

        stores.Count.Should().Be(take);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(11);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(20);
    }

    [Fact]
    public async Task ReturnsOrderStoresByNameDescForCompanyWithId2_GivenStoresByCompanyOrderedDescByNameSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var spec = new StoresByCompanyOrderedDescByNameSpec(2);

        var stores = await repo.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_LAST_ID);
    }

    [Fact]
    public async Task ReturnsOrderStoresByNameDescThenByIdForCompanyWithId2_GivenStoresByCompanyOrderedDescByNameThenByIdSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var spec = new StoresByCompanyOrderedDescByNameThenByIdSpec(2);

        var stores = await repo.ListAsync(spec);

        stores.First().Id.Should().Be(99);
        stores.Last().Id.Should().Be(98);
    }

    [Fact]
    public async Task ReturnsSecondPageOfStoresForCompanyWithId2_GivenStoresByCompanyPaginatedOrderedDescByNameSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var take = 10; // pagesize 10
        var skip = (2 - 1) * 10; // page 2

        var spec = new StoresByCompanyPaginatedOrderedDescByNameSpec(2, skip, take);

        var stores = await repo.ListAsync(spec);

        stores.Count.Should().Be(take);
        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_PAGE2_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_PAGE2_LAST_ID);
    }

    [Fact]
    public async Task ReturnsSecondPageOfStoresForCompanyWithId2_GivenStoresByCompanyPaginatedSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var take = 10; // pagesize 10
        var skip = (2 - 1) * 10; // page 2

        var spec = new StoresByCompanyPaginatedSpec(2, skip, take);

        var stores = await repo.ListAsync(spec);

        stores.Count.Should().Be(take);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(61);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(70);
    }

    [Fact]
    public async Task ReturnsOrderedStores_GivenStoresOrderedSpecByName()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var spec = new StoresOrderedSpecByName();

        var stores = await repo.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_LAST_ID);
    }

    [Fact]
    public async Task ReturnsOrderedStores_GivenStoresOrderedDescendingByNameSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var spec = new StoresOrderedDescendingByNameSpec();

        var stores = await repo.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_LAST_ID);
    }

    [Fact]
    public async Task ReturnsStoreContainingCity1_GivenStoreIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.ListAsync(new StoreSearchByNameOrCitySpec(StoreSeed.VALID_Search_City_Key));

        result.Should().NotBeNull();
        result.Should().ContainSingle();
        result[0].Id.Should().Be(StoreSeed.VALID_Search_ID);
        result[0].City.Should().Contain(StoreSeed.VALID_Search_City_Key);
    }

    [Fact]
    public virtual async Task ReturnsAllProducts_GivenStoreSelectManyProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var result = await repo.ListAsync(new StoreProductNamesSpec());

        result.Should().NotBeNull();
        result.Should().HaveCount(ProductSeed.TOTAL_PRODUCT_COUNT);
        result.OrderBy(x => x).First().Should().Be(ProductSeed.VALID_PRODUCT_NAME);
    }
}
