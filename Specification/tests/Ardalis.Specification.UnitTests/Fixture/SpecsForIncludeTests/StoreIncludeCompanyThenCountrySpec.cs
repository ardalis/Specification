namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeCompanyThenCountrySpec : Specification<Store>
{
    public StoreIncludeCompanyThenCountrySpec()
    {
        Query.Include(x => x.Company)
             .ThenInclude(x => x!.Country);
    }
}
