namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreNamesWithPostProcessingActionSpec : Specification<Store, string?>
{
    public StoreNamesWithPostProcessingActionSpec()
    {
        Query.Select(x => x.Name)
             .PostProcessingAction(x => x);
    }
}
