namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SearchExtensionTests(TestFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public void QueriesMatch_GivenSingleSearch()
    {
        var storeTerm = "ab1";
        var searchExpression = new SearchExpressionInfo<Store>(x => x.Name, $"%{storeTerm}%");

        var actual = DbContext.Stores
            .ApplySingleLike(searchExpression)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }

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
        var array = Array.Empty<SearchExpressionInfo<Store>>();

        var actual = DbContext.Stores
            .ApplyLikesAsOrGroup(array.AsEnumerable())
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
