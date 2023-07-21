namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeAddressSpec : Specification<Store>
{
    public StoreIncludeAddressSpec()
    {
        Query.Include(x => x.Address);
    }
}
