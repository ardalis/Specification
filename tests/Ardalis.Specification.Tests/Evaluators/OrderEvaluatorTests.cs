﻿namespace Tests.Evaluators;

public class OrderEvaluatorTests
{
    private static readonly OrderEvaluator _evaluator = OrderEvaluator.Instance;

    public record Customer(int Id, string? Name = null);

    [Fact]
    public void OrdersItemsAscending_GivenOrderBy()
    {
        List<Customer> input = [new(3), new(1), new(2), new(5), new(4)];
        List<Customer> expected = [new(1), new(2), new(3), new(4), new(5)];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(x => x.Id);

        Assert(spec, input, expected);
    }

    [Fact]
    public void OrdersItemsDescending_GivenOrderByDescending()
    {
        List<Customer> input = [new(3), new(1), new(2), new(5), new(4)];
        List<Customer> expected = [new(5), new(4), new(3), new(2), new(1)];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderByDescending(x => x.Id);

        Assert(spec, input, expected);
    }

    [Fact]
    public void OrdersItems_GivenOrderByThenBy()
    {
        List<Customer> input = [new(3, "c"), new(1, "b"), new(1, "a")];
        List<Customer> expected = [new(1, "a"), new(1, "b"), new(3, "c")];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Name);

        Assert(spec, input, expected);
    }

    [Fact]
    public void OrdersItems_GivenOrderByThenByDescending()
    {
        List<Customer> input = [new(3, "c"), new(1, "a"), new(1, "b")];
        List<Customer> expected = [new(1, "b"), new(1, "a"), new(3, "c")];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderBy(x => x.Id)
            .ThenByDescending(x => x.Name);

        Assert(spec, input, expected);
    }

    [Fact]
    public void OrdersItems_GivenOrderByDescendingThenBy()
    {
        List<Customer> input = [new(1, "b"), new(1, "a"), new(3, "c")];
        List<Customer> expected = [new(3, "c"), new(1, "a"), new(1, "b")];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderByDescending(x => x.Id)
            .ThenBy(x => x.Name);

        Assert(spec, input, expected);
    }

    [Fact]
    public void OrdersItems_GivenOrderByDescendingThenByDescending()
    {
        List<Customer> input = [new(1, "a"), new(1, "b"), new(3, "c")];
        List<Customer> expected = [new(3, "c"), new(1, "b"), new(1, "a")];

        var spec = new Specification<Customer>();
        spec.Query
            .OrderByDescending(x => x.Id)
            .ThenByDescending(x => x.Name);

        Assert(spec, input, expected);
    }

    [Fact]
    public void DoesNotOrder_GivenNoOrder()
    {
        List<Customer> input = [new(3), new(1), new(2), new(5), new(4)];
        List<Customer> expected = [new(3), new(1), new(2), new(5), new(4)];
        var spec = new Specification<Customer>();

        Assert(spec, input, expected);
    }

    private static void Assert<T>(Specification<T> spec, List<T> input, List<T> expected) where T : class
    {
        var actualForIEnumerable = _evaluator.Evaluate(input, spec);
        actualForIEnumerable.Should().NotBeNull();
        actualForIEnumerable.Should().Equal(expected);

        var actualForIQueryable = _evaluator.Evaluate(input.AsQueryable(), spec);
        actualForIQueryable.Should().NotBeNull();
        actualForIQueryable.Should().Equal(expected);
    }
}
