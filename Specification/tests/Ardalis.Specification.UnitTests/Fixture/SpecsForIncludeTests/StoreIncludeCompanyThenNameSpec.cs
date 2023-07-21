namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeCompanyThenNameSpec : Specification<Store>
{
    public StoreIncludeCompanyThenNameSpec()
    {
        Query.Include(x => x.Company)
             .ThenInclude(x => x!.Name);
    }
}
