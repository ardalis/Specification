namespace Tests.Repositories;

[Collection("SharedCollection")]
public class Repository_CountTests(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(string? Name);

    [Fact]
    public async Task CountAsync_ReturnsZero_GivenNoItems()
    {
        var repo = new Repository<Country>(DbContext);

        var result = await repo.CountAsync();

        result.Should().Be(0);
    }

    [Fact]
    public async Task CountAsync_ReturnsAllItemsCount()
    {
        var expected = new List<Country>
        {
            new() { Name = "a" },
            new() { Name = "b" },
            new() { Name = "c" },
        };
        await SeedRangeAsync(expected);

        var repo = new Repository<Country>(DbContext);

        var result = await repo.CountAsync();

        result.Should().Be(expected.Count);
    }

    [Fact]
    public async Task CountAsync_ReturnsFilteredItemCount_GivenSpec()
    {
        var expected = new List<Country>
        {
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "b" },
        };
        await SeedRangeAsync(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            .. expected,
            new() { Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b");

        var result = await repo.CountAsync(spec);

        result.Should().Be(expected.Count);
    }

    [Fact]
    public async Task CountAsync_ReturnsZero_GivenSpecAndNoMatch()
    {
        await SeedRangeAsync(new List<Country>
        {
            new() { Name = "a" },
            new() { Name = "c" },
            new() { Name = "d" },
        });

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b");

        var result = await repo.CountAsync(spec);

        result.Should().Be(0);
    }

    [Fact]
    public async Task CountAsync_ReturnsFilteredItemsCount_GivenProjectionSpec()
    {
        var expected = new List<CountryDto>
        {
            new("b"),
            new("b"),
            new("b"),
        };
        await SeedRangeAsync<Country>(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == "b")
            .Select(x => new CountryDto(x.Name));

        var result = await repo.CountAsync(spec);

        result.Should().Be(expected.Count);
    }

    [Fact]
    public async Task CountAsync_IgnoresPagination_GivenSpecWithPagination()
    {
        var expected = new List<Country>
        {
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "b" },
        };
        await SeedRangeAsync(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            .. expected,
            new() { Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == "b")
            .Skip(1)
            .Take(1);

        var result = await repo.CountAsync(spec);

        result.Should().Be(expected.Count);

        // Ensure that the spec's pagination is not altered.
        spec.Skip.Should().Be(1);
        spec.Take.Should().Be(1);
    }

    [Fact]
    public async Task CountAsync_IgnoresPagination_GivenProjectionSpecWithPagination()
    {
        var expected = new List<CountryDto>
        {
            new("b"),
            new("b"),
            new("b"),
        };
        await SeedRangeAsync<Country>(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "d" },
        ]);

        var repo = new Repository<Country>(DbContext);
        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == "b")
            .Skip(1)
            .Take(1)
            .Select(x => new CountryDto(x.Name));

        var result = await repo.CountAsync(spec);

        result.Should().Be(expected.Count);

        // Ensure that the spec's pagination is not altered.
        spec.Skip.Should().Be(1);
        spec.Take.Should().Be(1);
    }
}
