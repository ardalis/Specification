using Ardalis.Specification.UnitTests.Fixture.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;

public class TestDbContext : DbContext
{
    public TestDbContext(string connectionString) : base(connectionString)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<Store> Stores { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        modelBuilder.Entity<Company>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        modelBuilder.Entity<Address>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        modelBuilder.Entity<Product>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        modelBuilder.Entity<Store>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        modelBuilder.Entity<Store>().HasOptional(x => x.Address).WithRequired(x => x!.Store!);
    }
}
