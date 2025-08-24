using System.Data.Entity;

namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SearchExtensionTests(TestFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public void QueriesMatch_GivenSingleSearch()
    {
        var storeTerm = "%ab1%";
        var searchExpression = new SearchExpressionInfo<Store>(x => x.Name, storeTerm);

        var actual = DbContext.Stores.ApplySingleLike(searchExpression);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores
            .Where(x => DbFunctions.Like(x.Name, storeTerm));
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleSearchAsList()
    {
        var storeTerm = "%ab1%";
        var companyTerm = "%ab2%";

        var array = new SearchExpressionInfo<Store>[]
        {
            new(x => x.Name, storeTerm),
            new(x => x.Company.Name, companyTerm)
        };

        var actual = DbContext.Stores.ApplyLikesAsOrGroup(array.ToList(), 0, array.Length);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores
            .Where(x => DbFunctions.Like(x.Name, storeTerm)
                    || DbFunctions.Like(x.Company.Name, companyTerm));
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleSearchAsEnumerable()
    {
        var storeTerm = "%ab1%";
        var companyTerm = "%ab2%";

        var array = new SearchExpressionInfo<Store>[]
        {
            new(x => x.Name, storeTerm),
            new(x => x.Company.Name, companyTerm)
        };

        var actual = DbContext.Stores.ApplyLikesAsOrGroup(array.AsEnumerable());
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores
            .Where(x => DbFunctions.Like(x.Name, storeTerm)
                    || DbFunctions.Like(x.Company.Name, companyTerm));
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenEmptyAsList()
    {
        var array = Array.Empty<SearchExpressionInfo<Store>>();

        var actual = DbContext.Stores.ApplyLikesAsOrGroup(array.ToList(), 0, array.Length);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores.AsQueryable();
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenEmptyAsEnumerable()
    {
        var array = Array.Empty<SearchExpressionInfo<Store>>();

        var actual = DbContext.Stores.ApplyLikesAsOrGroup(array.AsEnumerable());
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores.AsQueryable();
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }
}
