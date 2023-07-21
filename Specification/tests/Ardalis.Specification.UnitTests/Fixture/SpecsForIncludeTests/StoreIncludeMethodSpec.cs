namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeMethodSpec : Specification<Store>
{
    public StoreIncludeMethodSpec()
    {
        Query.Include(x => Store.GetSomethingFromStore());
    }
}
