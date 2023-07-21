namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdSpec : Specification<Company>, ISingleResultSpecification
{
    public CompanyByIdSpec(int id)
    {
        Query.Where(company => company.Id == id);
    }
}
