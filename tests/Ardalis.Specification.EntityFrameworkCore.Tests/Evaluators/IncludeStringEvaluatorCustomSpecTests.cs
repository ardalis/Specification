namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class IncludeStringEvaluatorCustomSpecTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly IncludeStringEvaluator _evaluator = IncludeStringEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoIncludeString()
    {
        var spec = Substitute.For<ISpecification<Store>>();

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenIncludeString()
    {
        var spec = Substitute.For<ISpecification<Store>>();
        spec.IncludeStrings.Returns(
        [
            nameof(Address)
        ]);

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Include(nameof(Address))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleIncludeStrings()
    {
        var spec = Substitute.For<ISpecification<Store>>();
        spec.IncludeStrings.Returns(
        [
            nameof(Address),
            $"{nameof(Company)}.{nameof(Country)}"
        ]);

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Include(nameof(Address))
            .Include($"{nameof(Company)}.{nameof(Country)}")
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
