namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeCompanyThenCountryAsStringSpec : Specification<Store>
{
    public StoreIncludeCompanyThenCountryAsStringSpec()
    {
        Query.Include($"{nameof(Company)}.{nameof(Company.Country)}");
    }
}
