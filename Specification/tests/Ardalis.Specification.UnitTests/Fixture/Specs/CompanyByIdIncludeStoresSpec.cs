namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdIncludeStoresSpec : Specification<Company>, ISingleResultSpecification<Company>
{
    public CompanyByIdIncludeStoresSpec(int companyId)
    {
        Query.Where(x => x.Id == companyId).Include(x => x.Stores);
    }
}
