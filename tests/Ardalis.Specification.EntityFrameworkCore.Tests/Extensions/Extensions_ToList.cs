using System.Threading;

namespace Tests.Extensions;

[Collection("SharedCollection")]
public class Extensions_ToList(TestFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public async Task ToListAsync_GivenSpec()
    {
        var expectedName = "Country3";

        var countries = new List<Country>
        {
            new()
            {
                Name = "Country1",
            },
            new()
            {
                Name = "Country2",
            },
            new()
            {
                Name = expectedName,
            }
        };

        await SeedRangeAsync(countries);


        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == expectedName);

        var result = await DbContext
            .Countries
            .ToListAsync(spec, CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task ToListAsync_GivenSpecAndPostProcessingAction()
    {
        var expectedName = "Country3";

        var countries = new List<Country>
        {
            new()
            {
                Name = "Country1",
            },
            new()
            {
                Name = "Country2",
            },
            new()
            {
                Name = expectedName,
            }
        };

        await SeedRangeAsync(countries);


        var spec = new Specification<Country>();
        spec.Query
            .PostProcessingAction(x => x.Where(x => x.Name == expectedName));

        var result = await DbContext
            .Countries
            .ToListAsync(spec, CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task ToEnumerableAsync_GivenSpec()
    {
        var expectedName = "Country3";

        var countries = new List<Country>
        {
            new()
            {
                Name = "Country1",
            },
            new()
            {
                Name = "Country2",
            },
            new()
            {
                Name = expectedName,
            }
        };

        await SeedRangeAsync(countries);


        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Name == expectedName);

        var result = await DbContext
            .Countries
            .ToEnumerableAsync(spec, CancellationToken.None);

        result.Should().ContainSingle();
        result.First().Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task ToEnumerableAsync_GivenSpecAndPostProcessingAction()
    {
        var expectedName = "Country3";

        var countries = new List<Country>
        {
            new()
            {
                Name = "Country1",
            },
            new()
            {
                Name = "Country2",
            },
            new()
            {
                Name = expectedName,
            }
        };

        await SeedRangeAsync(countries);

        var spec = new Specification<Country>();
        spec.Query
            .PostProcessingAction(x => x.Where(x => x.Name == expectedName));

        var result = await DbContext
            .Countries
            .ToEnumerableAsync(spec, CancellationToken.None);

        result.Should().ContainSingle();
        result.First().Name.Should().Be(expectedName);
    }
}
