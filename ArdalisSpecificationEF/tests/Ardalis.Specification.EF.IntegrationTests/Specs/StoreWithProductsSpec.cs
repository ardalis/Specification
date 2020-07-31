using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Specs
{
    public class StoreWithProductsSpec : Specification<Store>
    {
        public StoreWithProductsSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Products);
        }
    }
}