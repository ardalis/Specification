namespace Tests.Evaluators;

public class WhereEvaluatorTests
{
    private static readonly WhereEvaluator _evaluator = WhereEvaluator.Instance;

    public record Customer(int Id);

    [Fact]
    public void ReturnTrue_IsCriteriaEvaluator()
    {
        _evaluator.IsCriteriaEvaluator.Should().BeTrue();
    }

    [Fact]
    public void Filters_GivenSingleWhereExpression()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 3);

        Assert(spec, input, expected);
    }

    [Fact]
    public void Filters_GivenMultipleWhereExpressions()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(4)];

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 3)
            .Where(x => x.Id < 5);

        Assert(spec, input, expected);
    }

    [Fact]
    public void DoesNotFilter_GivenNoWhereExpression()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();

        Assert(spec, input, expected);
    }

    private static void Assert<T>(Specification<T> spec, List<T> input, List<T> expected) where T : class
    {
        var actualForIEnumerable = _evaluator.Evaluate(input, spec);
        actualForIEnumerable.Should().NotBeNull();
        actualForIEnumerable.Should().Equal(expected);

        var actualForIQueryable = _evaluator.GetQuery(input.AsQueryable(), spec);
        actualForIQueryable.Should().NotBeNull();
        actualForIQueryable.Should().Equal(expected);
    }
}
