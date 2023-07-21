namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoresOrderedTwoChainsSpec : Specification<Store>
{
    public StoresOrderedTwoChainsSpec()
    {
        Query.OrderBy(x => x.Name)
            .OrderBy(x => x.Id);
    }
}
