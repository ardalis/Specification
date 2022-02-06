using System.Data.Entity.ModelConfiguration;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Configurations
{
  public class CountryConfiguration : EntityTypeConfiguration<Country>
  {
    public CountryConfiguration()
    {
      ToTable("Country");
      HasKey(c => c.Id);
    }
  }
}
