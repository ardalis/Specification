namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoresPaginatedSpec : Specification<Store>
{
    public StoresPaginatedSpec(int skip, int take)
    {
        Query.OrderBy(s => s.Id)
            .Skip(skip)
            .Take(take);
    }
}
