namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class AsSplitQueryEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly AsSplitQueryEvaluator _evaluator = AsSplitQueryEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenAsSplitQuery()
    {
        var spec = new Specification<Country>();
        spec.Query.AsSplitQuery();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .ToQueryString();

        var expected = DbContext.Countries
            .AsSplitQuery()
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Applies_GivenAsSplitQuery()
    {
        var spec = new Specification<Country>();
        spec.Query.AsSplitQuery();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsSplitQuery()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }
}
