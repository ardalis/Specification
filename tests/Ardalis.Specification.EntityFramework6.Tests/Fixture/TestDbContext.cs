using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Tests.Fixture;

public class TestDbContext : DbContext
{
    public TestDbContext(string connectionString) : base(connectionString)
    {
    }

    public virtual DbSet<Country> Countries { get; set; } = null!;
    public virtual DbSet<Company> Companies { get; set; } = null!;
    public virtual DbSet<Store> Stores { get; set; } = null!;
    public virtual DbSet<Address> Addresses { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;

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
