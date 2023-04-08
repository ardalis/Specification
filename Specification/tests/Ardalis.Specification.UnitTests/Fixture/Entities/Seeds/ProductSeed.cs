using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests.Fixture.Entities.Seeds
{
  public class ProductSeed
  {
    public const int TOTAL_PRODUCT_COUNT = 100;
    public const string VALID_PRODUCT_NAME = "Product 1";

    public static List<Product> Get()
    {
      var products = new List<Product>();

      for (int i = 1; i < TOTAL_PRODUCT_COUNT; i = i + 2)
      {
        products.Add(new Product()
        {
          Id = i,
          Name = $"Product {i}",
          StoreId = i,
        });
        products.Add(new Product()
        {
          Id = i + 1,
          Name = $"Product {i + 1}",
          StoreId = i,
        });
      }

      return products;
    }
  }
}
