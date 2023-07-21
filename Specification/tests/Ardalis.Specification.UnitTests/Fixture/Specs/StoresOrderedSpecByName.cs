namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoresOrderedSpecByName : Specification<Store>
{
    public StoresOrderedSpecByName()
    {
        Query.OrderBy(x => x.Name);
    }
}
