using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;

public class GetCompanyWithStoresSpec : Specification<Company>, ISingleResultSpecification<Company>
{
    public GetCompanyWithStoresSpec(int companyId)
    {
        Query.Where(x => x.Id == companyId).Include(x => x.Stores);
    }
}
