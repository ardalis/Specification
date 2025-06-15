using System.Data.Entity;

namespace Tests.FixtureNew;

public class TestDbContext : DbContext
{
    public TestDbContext(string connectionString) : base(connectionString)
    {
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // Store-Address one-to-one (Address has StoreId FK)
        modelBuilder.Entity<Store>()
            .HasOptional(s => s.Address)
            .WithRequired(a => a.Store)
            .Map(m => m.MapKey("StoreId"));

        // Country-Company one-to-many (Company has CountryId FK)
        modelBuilder.Entity<Company>()
            .HasRequired(co => co.Country)
            .WithMany()
            .HasForeignKey(co => co.CountryId);
    }
}
