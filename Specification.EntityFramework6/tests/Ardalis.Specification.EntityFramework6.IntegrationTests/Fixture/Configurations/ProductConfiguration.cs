using Ardalis.Specification.UnitTests.Fixture.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Configurations;

public class ProductConfiguration : EntityTypeConfiguration<Product>
{
  public ProductConfiguration()
  {
    ToTable("Product");
    HasKey(c => c.Id);
  }
}
