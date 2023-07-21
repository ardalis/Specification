namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreProductNamesSpec : Specification<Store, string?>
{
    public StoreProductNamesSpec()
    {
        Query.SelectMany(s => s.Products.Select(p => p.Name));
    }
}
