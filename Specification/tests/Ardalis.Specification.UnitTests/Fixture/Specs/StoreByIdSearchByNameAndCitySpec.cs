namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreByIdSearchByNameAndCitySpec : Specification<Store>
{
    public StoreByIdSearchByNameAndCitySpec(int id, string searchTerm)
    {
        Query.Where(x => x.Id == id)
            .Search(x => x.Name!, "%" + searchTerm + "%", 1)
            .Search(x => x.City!, "%" + searchTerm + "%", 2);
    }
}
