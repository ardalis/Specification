namespace Tests.Repositories;

[Collection("SharedCollection")]
public class Repository_WriteTests(TestFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        var repo = new Repository<Country>(DbContext);
        var country = new Country
        {
            Name = Guid.NewGuid().ToString(),
        };

        await repo.AddAsync(country);
        DbContext.ChangeTracker.Clear();

        var countriesInDb = await DbContext.Countries.IgnoreQueryFilters().ToListAsync();
        countriesInDb.Should().ContainSingle();
        countriesInDb.First().Name.Should().Be(country.Name);
    }

    [Fact]
    public async Task AddAsync_ShouldAddMultipleEntities()
    {
        var repo = new Repository<Country>(DbContext);
        var countries = new[]
        {
            new Country { Name = Guid.NewGuid().ToString() },
            new Country { Name = Guid.NewGuid().ToString() },
            new Country { Name = Guid.NewGuid().ToString() },
        };

        await repo.AddRangeAsync(countries);
        DbContext.ChangeTracker.Clear();

        var countriesInDb = await DbContext.Countries.ToListAsync();
        countriesInDb.Should().HaveCount(3);
        countriesInDb.Select(x => x.Name).Should().BeEquivalentTo(countries.Select(x => x.Name));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        var repo = new Repository<Country>(DbContext);
        var country = new Country
        {
            Name = Guid.NewGuid().ToString(),
        };
        await SeedAsync(country);

        country = await DbContext.Countries.FirstAsync();
        country.Name = Guid.NewGuid().ToString();
        await repo.UpdateAsync(country);
        DbContext.ChangeTracker.Clear();

        var countriesInDb = await DbContext.Countries.ToListAsync();
        countriesInDb.Should().NotBeNull();
        countriesInDb.Should().ContainSingle();
        countriesInDb.First().Name.Should().Be(country.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        var repo = new Repository<Country>(DbContext);
        var country = new Country
        {
            Name = Guid.NewGuid().ToString(),
        };
        await SeedAsync(country);

        var countryInDb = await DbContext.Countries.FirstAsync();
        await repo.DeleteAsync(countryInDb);
        DbContext.ChangeTracker.Clear();

        var countriesInDb = await DbContext.Countries.ToListAsync();
        countriesInDb.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteMultipleEntities()
    {
        var repo = new Repository<Country>(DbContext);
        var countries = new[]
        {
            new Country { Name = Guid.NewGuid().ToString() },
            new Country { Name = Guid.NewGuid().ToString() },
            new Country { Name = Guid.NewGuid().ToString() },
        };
        await SeedRangeAsync(countries);

        var countriesInDb = await DbContext.Countries.ToListAsync();
        await repo.DeleteRangeAsync(countriesInDb);
        DbContext.ChangeTracker.Clear();

        countriesInDb = await DbContext.Countries.ToListAsync();
        countriesInDb.Should().BeEmpty();
    }
}
