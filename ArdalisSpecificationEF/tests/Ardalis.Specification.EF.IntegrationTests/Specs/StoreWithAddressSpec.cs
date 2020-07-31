using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Specs
{
    public class StoreWithAddressSpec : Specification<Store>
    {
        public StoreWithAddressSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Address);
        }
    }
}