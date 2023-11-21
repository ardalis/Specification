using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;

public class ProductSeed
{
#pragma warning disable IDE1006 // Naming Styles
    public const int TOTAL_PRODUCT_COUNT = 100;
    public const string VALID_PRODUCT_NAME = "Product 1";
#pragma warning restore IDE1006 // Naming Styles

    public static List<Product> Get()
    {
        var products = new List<Product>();

        for (var i = 1; i < TOTAL_PRODUCT_COUNT; i += 2)
        {
            products.Add(new()
            {
                Id = i,
                Name = $"Product {i}",
                StoreId = i,
            });
            products.Add(new()
            {
                Id = i + 1,
                Name = $"Product {i + 1}",
                StoreId = i,
            });
        }

        return products;
    }
}
