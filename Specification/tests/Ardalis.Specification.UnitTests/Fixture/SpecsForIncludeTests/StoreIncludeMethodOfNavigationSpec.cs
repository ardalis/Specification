namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeMethodOfNavigationSpec : Specification<Store>
{
    public StoreIncludeMethodOfNavigationSpec()
    {
        Query.Include(x => x.Address!.GetSomethingFromAddress());
    }
}
