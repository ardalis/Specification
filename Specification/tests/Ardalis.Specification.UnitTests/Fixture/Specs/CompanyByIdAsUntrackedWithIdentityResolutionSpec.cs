namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdAsUntrackedWithIdentityResolutionSpec : Specification<Company>, ISingleResultSpecification
{
    public CompanyByIdAsUntrackedWithIdentityResolutionSpec(int id)
    {
        Query.Where(company => company.Id == id).AsNoTrackingWithIdentityResolution();
    }
}
