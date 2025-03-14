namespace Tests;

[Collection("ReadCollection")]
public class RepositoryOfT_ListAsync
{
    private readonly string _connectionString;

    public RepositoryOfT_ListAsync(TestFactory factory)
    {
        _connectionString = factory.ConnectionString;
    }

    [Fact]
    public async Task ReturnsStoreWithProducts_GivenStoreIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query.Include(x => x.Products);

        var result = await repo.ListAsync(spec);

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Products.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ReturnsStoreWithAddress_GivenStoreIncludeAddressSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query.Include(x => x.Address);

        var result = await repo.ListAsync(spec);

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result[0].Address.Should().NotBeNull();
    }

    [Fact]
    public async Task ReturnsStoreWithAddressAndProduct_GivenStoreIncludeAddressAndProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);
        var spec = new Specification<Store>();
        spec.Query
            .Include(x => x.Products)
             .Include(x => x!.Address);

        var result = await repo.ListAsync(spec);

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

        var spec = new Specification<Store>();
        spec.Query.Where(x => ids.Contains(x.Id));

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

        var spec = new Specification<Store, string?>();
        spec.Query
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(take)
            .Select(x => x.Name);

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

        var spec = new Specification<Store>();
        spec.Query
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(take);

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

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.CompanyId == 2)
            .OrderByDescending(x => x.Name);

        var stores = await repo.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_LAST_ID);
    }

    [Fact]
    public async Task ReturnsOrderStoresByNameDescThenByIdForCompanyWithId2_GivenStoresByCompanyOrderedDescByNameThenByIdSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.CompanyId == 2)
            .OrderByDescending(x => x.Name)
            .ThenBy(x => x.Id);

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

        var spec = new Specification<Store>();
        spec.Query.Where(x => x.CompanyId == 2)
             .Skip(skip)
             .Take(take)
             .OrderByDescending(x => x.Name);

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

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.CompanyId == 2)
            .OrderBy(x => x.CompanyId)
            .Skip(skip)
            .Take(take);

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

        var spec = new Specification<Store>();
        spec.Query.OrderBy(x => x.Name);

        var stores = await repo.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_LAST_ID);
    }

    [Fact]
    public async Task ReturnsOrderedStores_GivenStoresOrderedDescendingByNameSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var spec = new Specification<Store>();
        spec.Query.OrderByDescending(x => x.Name);

        var stores = await repo.ListAsync(spec);

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_LAST_ID);
    }

    [Fact]
    public async Task ReturnsStoreContainingCity1_GivenStoreIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_connectionString);
        var repo = new Repository<Store>(dbContext);

        var spec = new Specification<Store>();
        spec.Query
            .Search(x => x.Name!, "%" + StoreSeed.VALID_Search_City_Key + "%")
            .Search(x => x.City!, "%" + StoreSeed.VALID_Search_City_Key + "%");

        var result = await repo.ListAsync(spec);

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

        var spec = new Specification<Store, string?>();
        spec.Query
            .SelectMany(s => s.Products.Select(p => p.Name));

        var result = await repo.ListAsync(spec);

        result.Should().NotBeNull();
        result.Should().HaveCount(ProductSeed.TOTAL_PRODUCT_COUNT);
        result.OrderBy(x => x).First().Should().Be(ProductSeed.VALID_PRODUCT_NAME);
    }
}
