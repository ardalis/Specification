namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class OrderEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly OrderEvaluator _evaluator = OrderEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenOrder()
    {
        var spec = new Specification<Store>();
        spec.Query
            .OrderBy(x => x.Id);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .OrderBy(x => x.Id)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenOrderChain()
    {
        var spec = new Specification<Store>();
        spec.Query
            .OrderBy(x => x.Id)
                .ThenBy(x => x.Name);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .OrderBy(x => x.Id)
                .ThenBy(x => x.Name)
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
