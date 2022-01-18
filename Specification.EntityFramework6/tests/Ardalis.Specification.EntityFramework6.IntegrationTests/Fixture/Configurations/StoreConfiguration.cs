using System.Data.Entity.ModelConfiguration;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Configurations
{
  public class StoreConfiguration : EntityTypeConfiguration<Store>
  {
    public StoreConfiguration()
    {
      ToTable("Store");
      HasKey(c => c.Id);

      HasOptional(s => s.Address)
          .WithRequired(x => x.Store);
    }
  }
}
