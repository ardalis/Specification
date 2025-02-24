namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class WhereEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly WhereEvaluator _evaluator = WhereEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenWhereExpressions()
    {
        var id = 10;
        var name = "Country1";

        var spec = new Specification<Country>();
        spec.Query
            .Where(x => x.Id > id)
            .Where(x => x.Name == name);

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .ToQueryString();

        var expected = DbContext.Countries
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
