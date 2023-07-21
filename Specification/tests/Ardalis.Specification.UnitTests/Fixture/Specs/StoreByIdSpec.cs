namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreByIdSpec : Specification<Store>
{
    public StoreByIdSpec(int id)
    {
        Query.Where(x => x.Id == id);
    }
}
