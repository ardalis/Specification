namespace Tests.Fixture;

public class TestDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Bar> Bars => Set<Bar>();
    public DbSet<Foo> Foos => Set<Foo>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Store>()
            .HasOne(x => x.Address)
            .WithOne(x => x.Store)
            .HasForeignKey<Address>(x => x.StoreId);

        modelBuilder.Entity<Country>()
            .HasMany<Company>()
            .WithOne(x => x.Country)
            .HasForeignKey(x => x.CountryId);

        modelBuilder.Entity<Country>()
            .HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<BarDerived>()
            .HasBaseType<BarChild>();
    }
}
