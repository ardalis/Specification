namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreWithPostProcessingActionSpec : Specification<Store>
{
    public StoreWithPostProcessingActionSpec()
    {
        Query.PostProcessingAction(x => x);
    }
}
