namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoreIncludeCompanyCountryDotSeparatedSpec : Specification<Store>
{
    public StoreIncludeCompanyCountryDotSeparatedSpec()
    {
        Query.Include(x => x.Company!.Country);
    }
}
