namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreSearchByNameAndCitySpec : Specification<Store>
{
    public StoreSearchByNameAndCitySpec(string searchTerm)
    {
        Query.Search(x => x.Name!, "%" + searchTerm + "%", 1)
            .Search(x => x.City!, "%" + searchTerm + "%", 2);
    }
}
