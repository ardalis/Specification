namespace Tests.Evaluators;

public class PaginationEvaluatorTests
{
    private static readonly PaginationEvaluator _evaluator = PaginationEvaluator.Instance;

    public record Customer(int Id);

    [Fact]
    public void ReturnFalse_IsCriteriaEvaluator()
    {
        _evaluator.IsCriteriaEvaluator.Should().BeFalse();
    }

    [Fact]
    public void SkipsAndTakes_GivenSkipAndTake()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(3), new(4)];

        var spec = new Specification<Customer>();
        spec.Query.Skip(2).Take(2);

        Assert(spec, input, expected);
    }

    [Fact]
    public void Takes_GivenTakeOnly()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3)];

        var spec = new Specification<Customer>();
        spec.Query.Take(3);

        Assert(spec, input, expected);
    }

    [Fact]
    public void Skips_GivenSkipOnly()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query.Skip(3);

        Assert(spec, input, expected);
    }

    [Fact]
    public void DoesNotPaginate_GivenNoSkipOrTake()
    {
        List<Customer> input = [new(1), new(2), new(3), new(4), new(5)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();

        Assert(spec, input, expected);
    }

    [Fact]
    public void TakesAll_GivenTakeIsLarge()
    {
        List<Customer> input = [new(1), new(2), new(3)];
        List<Customer> expected = [new(1), new(2), new(3)];

        var spec = new Specification<Customer>();
        spec.Query.Take(100);

        Assert(spec, input, expected);
    }

    [Fact]
    public void TakesNone_GivenTakeIsZero()
    {
        List<Customer> input = [new(1), new(2), new(3)];
        List<Customer> expected = new();

        var spec = new Specification<Customer>();
        spec.Query.Take(0);

        Assert(spec, input, expected);
    }

    [Fact]
    public void TakesAll_GivenTakeIsNegative()
    {
        List<Customer> input = [new(1), new(2), new(3)];
        List<Customer> expected = [new(1), new(2), new(3)];

        var spec = new Specification<Customer>();
        spec.Query.Take(-1);

        Assert(spec, input, expected);
    }

    [Fact]
    public void SkipsNone_GivenSkipIsZero()
    {
        List<Customer> input = [new(1), new(2), new(3)];
        List<Customer> expected = [new(1), new(2), new(3)];

        var spec = new Specification<Customer>();
        spec.Query.Skip(0);

        Assert(spec, input, expected);
    }

    [Fact]
    public void SkipsNone_GivenSkipIsNegative()
    {
        List<Customer> input = [new(1), new(2), new(3)];
        List<Customer> expected = [new(1), new(2), new(3)];

        var spec = new Specification<Customer>();
        spec.Query.Skip(-1);

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
