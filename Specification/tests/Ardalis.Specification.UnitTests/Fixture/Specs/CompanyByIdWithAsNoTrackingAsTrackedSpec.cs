namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdWithAsNoTrackingAsTrackedSpec : Specification<Company>, ISingleResultSpecification<Company>
{
    public CompanyByIdWithAsNoTrackingAsTrackedSpec(int id)
    {
        Query.Where(company => company.Id == id).AsNoTracking().AsNoTrackingWithIdentityResolution().AsTracking();
    }
}
