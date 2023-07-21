using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactoryProvider.LoggerFactoryInstance);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Store>().HasOne(x => x.Address).WithOne(x => x!.Store!).HasForeignKey<Address>(x => x.StoreId);

        modelBuilder.Entity<Country>().HasData(CountrySeed.Get());
        modelBuilder.Entity<Company>().HasData(CompanySeed.Get());
        modelBuilder.Entity<Address>().HasData(AddressSeed.Get());
        modelBuilder.Entity<Store>().HasData(StoreSeed.Get());
        modelBuilder.Entity<Product>().HasData(ProductSeed.Get());
    }
}
