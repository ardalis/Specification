namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class TagWithEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly TagWithEvaluator _evaluator = TagWithEvaluator.Instance;

    [Fact]
    public void Applies_GivenTag()
    {
        var tag = "asd";

        var spec = new Specification<Country>();
        spec.Query.TagWith(tag);

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .TagWith(tag)
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }
}
