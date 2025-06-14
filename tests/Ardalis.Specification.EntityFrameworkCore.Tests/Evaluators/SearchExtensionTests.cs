namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SearchExtensionTests(TestFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public void QueriesMatch_GivenMultipleSearchAsSpan()
    {
        var storeTerm = "ab1";
        var companyTerm = "ab2";

        var array = new SearchExpressionInfo<Store>[]
        {
            new(x => x.Name, $"%{storeTerm}%"),
            new(x => x.Company.Name, $"%{companyTerm}%")
        };

        var actual = DbContext.Stores
            .ApplyLikesAsOrGroup(array)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleSearchAsEnumerable()
    {
        var storeTerm = "ab1";
        var companyTerm = "ab2";

        var array = new SearchExpressionInfo<Store>[]
        {
            new(x => x.Name, $"%{storeTerm}%"),
            new(x => x.Company.Name, $"%{companyTerm}%")
        };

        var actual = DbContext.Stores
            .ApplyLikesAsOrGroup(array.AsEnumerable())
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenEmptyAsSpan()
    {
        var spec = new Specification<Store>();

        var array = Array.Empty<SearchExpressionInfo<Store>>();

        var actual = DbContext.Stores
            .ApplyLikesAsOrGroup(array)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenEmptyAsEnumerable()
    {
        var spec = new Specification<Store>();

        var array = Array.Empty<SearchExpressionInfo<Store>>();

        var actual = DbContext.Stores
            .ApplyLikesAsOrGroup(array.AsEnumerable())
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
