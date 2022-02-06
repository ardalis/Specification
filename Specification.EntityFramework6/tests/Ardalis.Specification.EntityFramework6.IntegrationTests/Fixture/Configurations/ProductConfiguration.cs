using System.Data.Entity.ModelConfiguration;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Configurations
{
  public class ProductConfiguration : EntityTypeConfiguration<Product>
  {
    public ProductConfiguration()
    {
      ToTable("Product");
      HasKey(c => c.Id);
    }
  }
}
