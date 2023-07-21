namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdIgnoreQueryFilters : Specification<Company>, ISingleResultSpecification
{
    public CompanyByIdIgnoreQueryFilters(int id)
    {
        Query.Where(company => company.Id == id).IgnoreQueryFilters();
    }
}
