using System.Data.Entity.ModelConfiguration;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Configurations
{
  public class AddressConfiguration : EntityTypeConfiguration<Address>
  {
    public AddressConfiguration()
    {
      ToTable("Address");
      HasKey(c => c.Id);
    }
  }
}
