namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoresByCompanyOrderedDescByNameSpec : Specification<Store>
{
    public StoresByCompanyOrderedDescByNameSpec(int companyId)
    {
        Query.Where(x => x.CompanyId == companyId)
             .OrderByDescending(x => x.Name);
    }
}
