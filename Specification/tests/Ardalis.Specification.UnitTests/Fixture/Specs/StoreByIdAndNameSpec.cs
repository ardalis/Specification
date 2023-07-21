namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreByIdAndNameSpec : Specification<Store>
{
    public StoreByIdAndNameSpec(int id, string name)
    {
        Query.Where(x => x.Id == id)
            .Where(x => x.Name == name);
    }
}
