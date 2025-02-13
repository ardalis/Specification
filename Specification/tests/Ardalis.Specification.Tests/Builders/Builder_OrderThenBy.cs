namespace Tests.Builders;

public class Builder_OrderThenBy
{
    public record Customer(int Id, string FirstName, string LastName, string Email);

    [Fact]
    public void DoesNothing_GivenThenByWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName, false);

        spec1.OrderExpressions.Should().ContainSingle();
        spec1.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
        spec2.OrderExpressions.Should().ContainSingle();
        spec2.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
    }

    [Fact]
    public void DoesNothing_GivenThenByWithDiscardedTopChain()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName, false)
            .ThenBy(x => x.LastName);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName, false)
            .ThenBy(x => x.LastName);

        spec1.OrderExpressions.Should().BeEmpty();
        spec2.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenThenByWithDiscardedNestedChain()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName, false)
            .ThenBy(x => x.Email);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName, false)
            .ThenBy(x => x.Email);

        spec1.OrderExpressions.Should().ContainSingle();
        spec1.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
        spec2.OrderExpressions.Should().ContainSingle();
        spec2.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
    }

    [Fact]
    public void AddsThenBy_GivenThenBy()
    {
        Expression<Func<Customer, object?>> expr = x => x.LastName;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(expr);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(expr);

        spec1.OrderExpressions.Should().HaveCount(2);
        spec1.OrderExpressions.Last().KeySelector.Should().BeSameAs(expr);
        spec1.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec1.OrderExpressions.Last().OrderType.Should().Be(OrderTypeEnum.ThenBy);
        spec2.OrderExpressions.Should().HaveCount(2);
        spec2.OrderExpressions.Last().KeySelector.Should().BeSameAs(expr);
        spec2.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec2.OrderExpressions.Last().OrderType.Should().Be(OrderTypeEnum.ThenBy);
    }

    [Fact]
    public void AddsThenBy_GivenMultipleThenBy()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ThenBy(x => x.Email);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ThenBy(x => x.Email);

        spec1.OrderExpressions.Should().HaveCount(3);
        spec1.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec1.OrderExpressions.Skip(1).Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.ThenBy));
        spec2.OrderExpressions.Should().HaveCount(3);
        spec2.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec2.OrderExpressions.Skip(1).Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.ThenBy));
    }

    [Fact]
    public void AddsThenBy_GivenThenByThenByDescending()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ThenByDescending(x => x.Email);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ThenByDescending(x => x.Email);

        spec1.OrderExpressions.Should().HaveCount(3);
        spec1.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec1.OrderExpressions.Skip(1).First().OrderType.Should().Be(OrderTypeEnum.ThenBy);
        spec1.OrderExpressions.Last().OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
        spec2.OrderExpressions.Should().HaveCount(3);
        spec2.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec2.OrderExpressions.Skip(1).First().OrderType.Should().Be(OrderTypeEnum.ThenBy);
        spec2.OrderExpressions.Last().OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
    }
}
