namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class AsTrackingEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly AsTrackingEvaluator _evaluator = AsTrackingEvaluator.Instance;

    [Fact]
    public void Applies_GivenAsTracking()
    {
        var spec = new Specification<Country>();
        spec.Query.AsTracking();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsTracking()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }
}
