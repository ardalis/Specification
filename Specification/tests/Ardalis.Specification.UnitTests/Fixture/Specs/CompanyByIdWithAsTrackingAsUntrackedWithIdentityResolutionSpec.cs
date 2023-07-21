namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdWithAsTrackingAsUntrackedWithIdentityResolutionSpec : Specification<Company>,
  ISingleResultSpecification<Company>
{
    public CompanyByIdWithAsTrackingAsUntrackedWithIdentityResolutionSpec(int id)
    {
        Query.Where(company => company.Id == id).AsTracking().AsNoTrackingWithIdentityResolution();
    }
}
