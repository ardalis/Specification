namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class ProductByIdAsUntrackedSpec : Specification<Product>, ISingleResultSpecification
{
    public ProductByIdAsUntrackedSpec(int id)
    {
        Query.Where(product => product.Id == id).AsNoTracking();
    }
}
