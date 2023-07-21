namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeProductsSpec : Specification<Store>
{
    public StoreIncludeProductsSpec()
    {
        Query.Include(x => x.Products);
    }
}
