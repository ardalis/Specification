using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using System;

namespace Ardalis.Specification.UnitTests;

public class InMemorySpecificationEvaluatorTests
{
    [Fact]
    public void ReturnsStoreWithId10_GivenStoreByIdSpec()
    {
        var spec = new StoreByIdSpec(10);

        var store = spec.Evaluate(StoreSeed.Get()).FirstOrDefault();

        store?.Id.Should().Be(10);
    }

    [Fact]
    public void ReturnsStoreWithIdFrom15To30_GivenStoresByIdListSpec()
    {
        var ids = Enumerable.Range(15, 16);
        var spec = new StoresByIdListSpec(ids);

        var stores = spec.Evaluate(StoreSeed.Get());

        stores.Count().Should().Be(16);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(15);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(30);
    }

    [Fact]
    public void ReturnsSecondPageOfStoreNames_GivenStoreNamesPaginatedSpec()
    {
        var take = 10; // pagesize 10
        var skip = (2 - 1) * 10; // page 2

        var spec = new StoreNamesPaginatedSpec(skip, take);

        var storeNames = spec.Evaluate(StoreSeed.Get());

        storeNames.Count().Should().Be(take);
        storeNames.First().Should().Be("Store 11");
        storeNames.Last().Should().Be("Store 20");
    }

    [Fact]
    public void ReturnsSecondPageOfStores_GivenStoresPaginatedSpec()
    {
        var take = 10; // pagesize 10
        var skip = (2 - 1) * 10; // page 2

        var spec = new StoresPaginatedSpec(skip, take);

        var stores = spec.Evaluate(StoreSeed.Get());

        stores.Count().Should().Be(take);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(11);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(20);
    }

    [Fact]
    public void ReturnsOrderStoresByNameDescForCompanyWithId2_GivenStoresByCompanyOrderedDescByNameSpec()
    {
        var spec = new StoresByCompanyOrderedDescByNameSpec(2);

        var stores = spec.Evaluate(StoreSeed.Get());

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_LAST_ID);
    }

    [Fact]
    public void ReturnsOrderStoresByNameDescThenByIdForCompanyWithId2_GivenStoresByCompanyOrderedDescByNameThenByIdSpec()
    {
        var spec = new StoresByCompanyOrderedDescByNameThenByIdSpec(2);

        var stores = spec.Evaluate(StoreSeed.Get());

        stores.First().Id.Should().Be(99);
        stores.Last().Id.Should().Be(98);
    }

    [Fact]
    public void ReturnsSecondPageOfStoresForCompanyWithId2_GivenStoresByCompanyPaginatedOrderedDescByNameSpec()
    {
        var take = 10; // pagesize 10
        var skip = (2 - 1) * 10; // page 2

        var spec = new StoresByCompanyPaginatedOrderedDescByNameSpec(2, skip, take);

        var stores = spec.Evaluate(StoreSeed.Get());

        stores.Count().Should().Be(take);
        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_PAGE2_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FOR_COMPANY2_PAGE2_LAST_ID);
    }

    [Fact]
    public void ReturnsSecondPageOfStoresForCompanyWithId2_GivenStoresByCompanyPaginatedSpec()
    {
        var take = 10; // pagesize 10
        var skip = (2 - 1) * 10; // page 2

        var spec = new StoresByCompanyPaginatedSpec(2, skip, take);

        var stores = spec.Evaluate(StoreSeed.Get());

        stores.Count().Should().Be(take);
        stores.OrderBy(x => x.Id).First().Id.Should().Be(61);
        stores.OrderBy(x => x.Id).Last().Id.Should().Be(70);
    }

    [Fact]
    public void ReturnsOrderedStores_GivenStoresOrderedSpecByName()
    {
        var spec = new StoresOrderedSpecByName();

        var stores = spec.Evaluate(StoreSeed.Get());

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_LAST_ID);
    }

    [Fact]
    public void ReturnsOrderedStores_GivenStoresOrderedDescendingByNameSpec()
    {
        var spec = new StoresOrderedDescendingByNameSpec();

        var stores = spec.Evaluate(StoreSeed.Get());

        stores.First().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_FIRST_ID);
        stores.Last().Id.Should().Be(StoreSeed.ORDERED_BY_NAME_DESC_LAST_ID);
    }

    [Fact]
    public void ThrowsDuplicateOrderChainException_GivenSpecWithMultipleOrderChains()
    {
        var spec = new StoresOrderedTwoChainsSpec();

        Action sutAction = () => spec.Evaluate(StoreSeed.Get());

        sutAction.Should()
            .Throw<DuplicateOrderChainException>()
            .WithMessage("The specification contains more than one Order chain!");
    }
}
