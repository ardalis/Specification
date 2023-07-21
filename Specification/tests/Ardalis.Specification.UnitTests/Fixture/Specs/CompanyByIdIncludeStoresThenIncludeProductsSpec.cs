namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdIncludeStoresThenIncludeProductsSpec : Specification<Company>, ISingleResultSpecification
{
    public CompanyByIdIncludeStoresThenIncludeProductsSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Stores)
            .ThenInclude(x => x.Products);
    }
}
