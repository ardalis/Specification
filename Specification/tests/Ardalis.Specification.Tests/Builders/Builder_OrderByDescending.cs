namespace Tests.Builders;

public class Builder_OrderByDescending
{
    public record Customer(int Id, string FirstName, string LastName);

    [Fact]
    public void DoesNothing_GivenNoOrderByDescending()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.OrderExpressions.Should().BeEmpty();
        spec2.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenOrderByDescendingWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderByDescending(x => x.FirstName, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderByDescending(x => x.FirstName, false);

        spec1.OrderExpressions.Should().BeEmpty();
        spec2.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsOrderByDescending_GivenOrderByDescending()
    {
        Expression<Func<Customer, object?>> expr = x => x.FirstName;
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderByDescending(expr);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderByDescending(expr);

        spec1.OrderExpressions.Should().ContainSingle();
        spec1.OrderExpressions.First().KeySelector.Should().BeSameAs(expr);
        spec1.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderByDescending);
        spec2.OrderExpressions.Should().ContainSingle();
        spec2.OrderExpressions.First().KeySelector.Should().BeSameAs(expr);
        spec2.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderByDescending);
    }

    [Fact]
    public void AddsOrderByDescending_GivenMultipleOrderByDescending()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderByDescending(x => x.FirstName)
            .OrderByDescending(x => x.LastName);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderByDescending(x => x.FirstName)
            .OrderByDescending(x => x.LastName);

        spec1.OrderExpressions.Should().HaveCount(2);
        spec1.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderByDescending));
        spec2.OrderExpressions.Should().HaveCount(2);
        spec2.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderByDescending));
    }
}
