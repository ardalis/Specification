namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreByIdIncludeAddressAndProductsSpec : Specification<Store>, ISingleResultSpecification
{
    public StoreByIdIncludeAddressAndProductsSpec(int id)
    {
        Query.Where(x => x.Id == id);
        Query.Include(x => x.Address);
        Query.Include(x => x.Products);
    }
}
