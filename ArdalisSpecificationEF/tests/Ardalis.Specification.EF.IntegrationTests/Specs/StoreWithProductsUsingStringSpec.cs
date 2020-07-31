using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Specs
{
    public class StoreWithProductsUsingStringSpec : Specification<Store>
    {
        public StoreWithProductsUsingStringSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(nameof(Store.Products));
        }
    }
}