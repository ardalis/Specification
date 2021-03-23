using Ardalis.Specification.UnitTests.Fixture.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Configurations
{
    public class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            ToTable("Company");
            HasKey(c => c.Id);

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
                
            //HasMany(c => c.Stores)
            //    .WithRequired(s => s.Company);
        }
    }
}