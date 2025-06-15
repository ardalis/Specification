using System.Data.Entity;

namespace Tests.FixtureNew;

public class TestDbContext : DbContext
{
    public TestDbContext(string connectionString) : base(connectionString)
    {
    }

    public DbSet<Bar> Bars => Set<Bar>();
    public DbSet<Foo> Foos => Set<Foo>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Store>()
            .HasOptional(x => x.Address)
            .WithRequired(x => x.Store);

        //modelBuilder.Entity<Store>()

        //    .HasOne(x => x.Address)
        //    .WithOne(x => x.Store)
        //    .HasForeignKey<Address>(x => x.StoreId);

        //modelBuilder.Entity<Country>()
        //    .HasMany<Company>()
        //    .WithOne(x => x.Country)
        //    .HasForeignKey(x => x.CountryId);

        //modelBuilder.Entity<Country>()
        //    .HasQueryFilter(x => !x.IsDeleted);

        //modelBuilder.Entity<BarDerived>()
        //    .HasBaseType<BarChild>();
    }
}
