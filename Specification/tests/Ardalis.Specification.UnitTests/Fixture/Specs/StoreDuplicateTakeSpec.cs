namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreDuplicateTakeSpec : Specification<Store>
{
    public StoreDuplicateTakeSpec()
    {
        Query.Take(1)
             .Take(2);
    }
}
