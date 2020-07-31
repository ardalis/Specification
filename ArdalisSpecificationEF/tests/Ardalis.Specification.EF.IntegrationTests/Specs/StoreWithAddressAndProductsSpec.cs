using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Specs
{
    public class StoreWithAddressAndProductsSpec : Specification<Store>
    {
        public StoreWithAddressAndProductsSpec(int id)
        {
            Query.Where(x => x.Id == id);
            Query.Include(x => x.Address);
            Query.Include(x => x.Products);
        }
    }
}
