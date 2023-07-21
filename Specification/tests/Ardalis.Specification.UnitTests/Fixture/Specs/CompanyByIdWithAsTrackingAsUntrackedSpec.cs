namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdWithAsTrackingAsUntrackedSpec : Specification<Company>, ISingleResultSpecification<Company>
{
    public CompanyByIdWithAsTrackingAsUntrackedSpec(int id)
    {
        Query.Where(company => company.Id == id).AsTracking().AsNoTracking();
    }
}
