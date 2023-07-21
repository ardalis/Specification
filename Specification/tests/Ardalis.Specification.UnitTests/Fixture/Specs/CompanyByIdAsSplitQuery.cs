namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdAsSplitQuery : Specification<Company>, ISingleResultSpecification
{
    public CompanyByIdAsSplitQuery(int id)
    {
        Query.Where(company => company.Id == id)
            .Include(x => x.Stores)
            .ThenInclude(x => x.Products)
            .AsSplitQuery();
    }
}
