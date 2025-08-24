namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class IncludeStringEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly IncludeStringEvaluator _evaluator = IncludeStringEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoIncludeString()
    {
        var spec = new Specification<Store>();

        var actual = _evaluator.GetQuery(DbContext.Stores, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores.AsQueryable();
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenIncludeString()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Include(nameof(Address));

        var actual = _evaluator.GetQuery(DbContext.Stores, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores
            .Include(nameof(Address));
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleIncludeStrings()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Include(nameof(Address))
            .Include($"{nameof(Company)}.{nameof(Country)}");

        var actual = _evaluator.GetQuery(DbContext.Stores, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores
            .Include(nameof(Address))
            .Include($"{nameof(Company)}.{nameof(Country)}");
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }
}
