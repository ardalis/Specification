using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Specs
{
    public class CompanyWithStoresThenIncludeAddressSpec : Specification<Company>
    {
        public CompanyWithStoresThenIncludeAddressSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Stores)
                .ThenInclude(x => x.Address);
        }
    }
}