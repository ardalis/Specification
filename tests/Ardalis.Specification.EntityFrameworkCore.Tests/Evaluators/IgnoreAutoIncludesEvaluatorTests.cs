namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class IgnoreAutoIncludesEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly IgnoreAutoIncludesEvaluator _evaluator = IgnoreAutoIncludesEvaluator.Instance;

    [Fact]
    public void Applies_GivenIgnoreAutoIncludes()
    {
        var spec = new Specification<Country>();
        spec.Query.IgnoreAutoIncludes();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .IgnoreAutoIncludes()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }
}
