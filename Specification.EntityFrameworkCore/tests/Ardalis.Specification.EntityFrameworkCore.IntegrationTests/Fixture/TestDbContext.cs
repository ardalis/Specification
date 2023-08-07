using Ardalis.Specification.UnitTests.Fixture.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;

public class TestDbContext : DbContext
{
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Product> Products => Set<Product>();

    public TestDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<Company>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<Address>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<Product>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<Store>().Property(x => x.Id).ValueGeneratedNever();

        modelBuilder.Entity<Store>().HasOne(x => x.Address).WithOne(x => x!.Store!).HasForeignKey<Address>(x => x.StoreId);
    }
}
