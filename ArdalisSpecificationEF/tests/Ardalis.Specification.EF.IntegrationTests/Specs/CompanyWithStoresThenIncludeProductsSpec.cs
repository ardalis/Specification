using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Specs
{
    public class CompanyWithStoresThenIncludeProductsSpec : Specification<Company>
    {
        public CompanyWithStoresThenIncludeProductsSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Stores)
                .ThenInclude(x => x.Products);
        }
    }
}
