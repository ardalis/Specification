namespace Tests.Repositories;

[Collection("SharedCollection")]
public class Repository_FirstTests(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(string? Name);

    [Fact]
    public async Task FirstOrDefaultAsync_ReturnsFirstItem_GivenEntityExists()
    {
        var expected = new Country { Name = "b" };
        await SeedRangeAsync(new[]
        {
            new Country { Name = "a" },
            expected,
            new Country { Name = "c" },
        });

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == expected.Name);

        var result = await repo.FirstOrDefaultAsync(spec);

        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ReturnsNull_GivenEntityNotExists()
    {
        var expected = new Country { Name = "b" };

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == expected.Name);

        var result = await repo.FirstOrDefaultAsync(spec);

        result.Should().BeNull();
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ReturnsFirstItem_GivenProjectionAndEntityExists()
    {
        var expected = new CountryDto("b");
        await SeedRangeAsync(new[]
        {
            new Country { Name = "a" },
            new Country { Name = expected.Name },
            new Country { Name = "c" },
        });

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == expected.Name)
            .Select(x => new CountryDto(x.Name));

        var result = await repo.FirstOrDefaultAsync(spec);

        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ReturnsNull_GivenProjectionAndEntityNotExists()
    {
        var expected = new CountryDto("b");

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == expected.Name)
            .Select(x => new CountryDto(x.Name));

        var result = await repo.FirstOrDefaultAsync(spec);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ReturnsFirstItem_GivenEntityExists()
    {
        var expected = new Country { Name = "b" };
        await SeedRangeAsync(new[]
        {
            new Country { Name = "a" },
            expected,
            new Country { Name = "c" },
        });

        var repo = new Repository<Country>(DbContext);
        var spec = new SingleResultSpecification<Country>();
        spec.Query
            .Where(x => x.Name == expected.Name);

        var result = await repo.SingleOrDefaultAsync(spec);

        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ReturnsNull_GivenEntityNotExists()
    {
        var expected = new Country { Name = "b" };

        var repo = new Repository<Country>(DbContext);
        var spec = new SingleResultSpecification<Country>();
        spec.Query
            .Where(x => x.Name == expected.Name);

        var result = await repo.SingleOrDefaultAsync(spec);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ThrowsException_GivenMultipleEntitiesExist()
    {
        var expected = new Country { Name = "b" };
        await SeedRangeAsync(new[]
        {
            new Country { Name = "a" },
            new Country { Name = expected.Name },
            new Country { Name = expected.Name },
            new Country { Name = "c" },
        });

        var repo = new Repository<Country>(DbContext);
        var spec = new SingleResultSpecification<Country>();
        spec.Query
            .Where(x => x.Name == expected.Name);

        var result = () => repo.SingleOrDefaultAsync(spec);

        await result.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ReturnsFirstItem_GivenProjectionAndEntityExists()
    {
        var expected = new CountryDto("b");
        await SeedRangeAsync(new[]
        {
            new Country { Name = "a" },
            new Country { Name = expected.Name },
            new Country { Name = "c" },
        });

        var repo = new Repository<Country>(DbContext);
        var spec = new SingleResultSpecification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == expected.Name)
            .Select(x => new CountryDto(x.Name));

        var result = await repo.SingleOrDefaultAsync(spec);

        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ReturnsNull_GivenProjectionAndEntityNotExists()
    {
        var expected = new CountryDto("b");

        var repo = new Repository<Country>(DbContext);
        var spec = new SingleResultSpecification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == expected.Name)
            .Select(x => new CountryDto(x.Name));

        var result = await repo.SingleOrDefaultAsync(spec);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ThrowsException_GivenProjectionMultipleEntitiesExist()
    {
        var expected = new CountryDto("b");
        await SeedRangeAsync(new[]
        {
            new Country { Name = "a" },
            new Country { Name = expected.Name },
            new Country { Name = expected.Name },
            new Country { Name = "c" },
        });

        var repo = new Repository<Country>(DbContext);
        var spec = new SingleResultSpecification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == expected.Name)
            .Select(x => new CountryDto(x.Name));

        var result = () => repo.SingleOrDefaultAsync(spec);

        await result.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task FindAsync_ReturnsFirstItem_GivenIdExists()
    {
        var expected = new Country { Name = "b" };
        await SeedRangeAsync(new[]
        {
            new Country { Name = "a" },
            expected,
            new Country { Name = "c" },
        });

        var repo = new Repository<Country>(DbContext);

        var result = await repo.GetByIdAsync(expected.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
    }

    [Fact]
    public async Task FindAsync_ReturnsFromChangeTracker_GivenItemIsLoaded()
    {
        var expected = new Country { Name = "b" };
        await SeedRangeAsync(new[]
        {
            new Country { Name = "a" },
            expected,
            new Country { Name = "c" },
        });

        var repo = new Repository<Country>(DbContext);

        var countryInTracker = await DbContext.Countries.FirstAsync(x => x.Id == expected.Id);
        var result = await repo.GetByIdAsync(expected.Id);

        result.Should().NotBeNull();
        result.Should().BeSameAs(countryInTracker);
    }

    [Fact]
    public async Task FindAsync_ReturnsNull_GivenIdNotExists()
    {
        var repo = new Repository<Country>(DbContext);

        var result = await repo.GetByIdAsync(99);

        result.Should().BeNull();
    }
}
