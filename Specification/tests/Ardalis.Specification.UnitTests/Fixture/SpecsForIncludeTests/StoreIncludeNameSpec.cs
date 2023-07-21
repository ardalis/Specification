namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeNameSpec : Specification<Store>
{
    public StoreIncludeNameSpec()
    {
        Query.Include(x => x.Name);
    }
}
