namespace Tests.Builders;

public class Builder_OrderThenByDescending
{
    public record Customer(int Id, string FirstName, string LastName, string Email);

    [Fact]
    public void DoesNothing_GivenThenByDescendingWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(x => x.LastName, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(x => x.LastName, false);

        spec1.OrderExpressions.Should().ContainSingle();
        spec1.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
        spec2.OrderExpressions.Should().ContainSingle();
        spec2.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
    }

    [Fact]
    public void DoesNothing_GivenThenByDescendingWithDiscardedTopChain()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName, false)
            .ThenByDescending(x => x.LastName);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName, false)
            .ThenByDescending(x => x.LastName);

        spec1.OrderExpressions.Should().BeEmpty();
        spec2.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenThenByDescendingWithDiscardedNestedChain()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(x => x.LastName, false)
            .ThenByDescending(x => x.Email);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(x => x.LastName, false)
            .ThenByDescending(x => x.Email);

        spec1.OrderExpressions.Should().ContainSingle();
        spec1.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
        spec2.OrderExpressions.Should().ContainSingle();
        spec2.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
    }

    [Fact]
    public void AddsThenByDescending_GivenThenByDescending()
    {
        Expression<Func<Customer, object?>> expr = x => x.LastName;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(expr);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(expr);

        spec1.OrderExpressions.Should().HaveCount(2);
        spec1.OrderExpressions.Last().KeySelector.Should().BeSameAs(expr);
        spec1.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec1.OrderExpressions.Last().OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
        spec2.OrderExpressions.Should().HaveCount(2);
        spec2.OrderExpressions.Last().KeySelector.Should().BeSameAs(expr);
        spec2.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec2.OrderExpressions.Last().OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
    }

    [Fact]
    public void AddsThenByDescending_GivenMultipleThenByDescending()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(x => x.LastName)
            .ThenByDescending(x => x.Email);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(x => x.LastName)
            .ThenByDescending(x => x.Email);

        spec1.OrderExpressions.Should().HaveCount(3);
        spec1.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec1.OrderExpressions.Skip(1).Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.ThenByDescending));
        spec2.OrderExpressions.Should().HaveCount(3);
        spec2.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec2.OrderExpressions.Skip(1).Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.ThenByDescending));
    }

    [Fact]
    public void AddsThenBy_GivenThenByDescendingThenBy()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(x => x.LastName)
            .ThenBy(x => x.Email);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenByDescending(x => x.LastName)
            .ThenBy(x => x.Email);

        spec1.OrderExpressions.Should().HaveCount(3);
        spec1.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec1.OrderExpressions.Skip(1).First().OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
        spec1.OrderExpressions.Last().OrderType.Should().Be(OrderTypeEnum.ThenBy);
        spec2.OrderExpressions.Should().HaveCount(3);
        spec2.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec2.OrderExpressions.Skip(1).First().OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
        spec2.OrderExpressions.Last().OrderType.Should().Be(OrderTypeEnum.ThenBy);
    }
}
