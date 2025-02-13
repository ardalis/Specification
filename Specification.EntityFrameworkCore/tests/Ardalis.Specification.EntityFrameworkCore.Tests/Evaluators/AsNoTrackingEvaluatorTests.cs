namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class AsNoTrackingEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly AsNoTrackingEvaluator _evaluator = AsNoTrackingEvaluator.Instance;

    [Fact]
    public void Applies_GivenAsNoTracking()
    {
        var spec = new Specification<Country>();
        spec.Query.AsNoTracking();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsNoTracking()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }
}
