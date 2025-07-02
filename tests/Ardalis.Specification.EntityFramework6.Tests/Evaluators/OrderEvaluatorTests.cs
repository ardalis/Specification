namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class OrderEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly Ardalis.Specification.EntityFramework6.OrderEvaluator _evaluator =
        Ardalis.Specification.EntityFramework6.OrderEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenOrder()
    {
        var spec = new Specification<Company>();
        spec.Query
            .OrderBy(x => x.Id);

        var actual = _evaluator.GetQuery(DbContext.Companies, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Companies
            .OrderBy(x => x.Id);
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenOrderChain()
    {
        var spec = new Specification<Company>();
        spec.Query
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Name);

        var actual = _evaluator.GetQuery(DbContext.Companies, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Companies
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Name);
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }
}
