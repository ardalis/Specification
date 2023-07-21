namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreNamesSpec : Specification<Store, string?>
{
    public StoreNamesSpec()
    {
        Query.Select(x => x.Name);
    }
}
