namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class ProductByIdAsTrackedSpec : Specification<Product>, ISingleResultSpecification<Product>
{
    public ProductByIdAsTrackedSpec(int id)
    {
        Query.Where(product => product.Id == id).AsTracking();
    }
}
