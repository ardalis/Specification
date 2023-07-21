using Ardalis.Specification.UnitTests.Fixture.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Configurations;

public class CountryConfiguration : EntityTypeConfiguration<Country>
{
  public CountryConfiguration()
  {
    ToTable("Country");
    HasKey(c => c.Id);
  }
}
