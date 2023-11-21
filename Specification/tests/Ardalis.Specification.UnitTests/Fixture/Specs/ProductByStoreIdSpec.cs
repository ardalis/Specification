namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class ProductByStoreIdSpec : Specification<Product>
{
    public ProductByStoreIdSpec(int storeId)
    {
        Query.Where(x => x.StoreId == storeId);
    }
}
