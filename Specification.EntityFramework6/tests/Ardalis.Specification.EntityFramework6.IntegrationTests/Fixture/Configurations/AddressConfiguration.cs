using Ardalis.Specification.UnitTests.Fixture.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Configurations;

public class AddressConfiguration : EntityTypeConfiguration<Address>
{
  public AddressConfiguration()
  {
    ToTable("Address");
    HasKey(c => c.Id);
  }
}
