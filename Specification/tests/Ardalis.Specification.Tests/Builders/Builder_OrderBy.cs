namespace Tests.Builders;

public class Builder_OrderBy
{
    public record Customer(int Id, string FirstName, string LastName);

    [Fact]
    public void DoesNothing_GivenNoOrderBy()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.OrderExpressions.Should().BeEmpty();
        spec2.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenOrderByWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName, false);

        spec1.OrderExpressions.Should().BeEmpty();
        spec2.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsOrderBy_GivenOrderBy()
    {
        Expression<Func<Customer, object?>> expr = x => x.FirstName;
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(expr);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(expr);

        spec1.OrderExpressions.Should().ContainSingle();
        spec1.OrderExpressions.First().KeySelector.Should().BeSameAs(expr);
        spec1.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec2.OrderExpressions.Should().ContainSingle();
        spec2.OrderExpressions.First().KeySelector.Should().BeSameAs(expr);
        spec2.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
    }

    [Fact]
    public void AddsOrderBy_GivenMultipleOrderBy()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .OrderBy(x => x.FirstName)
            .OrderBy(x => x.LastName);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .OrderBy(x => x.FirstName)
            .OrderBy(x => x.LastName);

        spec1.OrderExpressions.Should().HaveCount(2);
        spec1.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
        spec2.OrderExpressions.Should().HaveCount(2);
        spec2.OrderExpressions.Should().AllSatisfy(x => x.OrderType.Should().Be(OrderTypeEnum.OrderBy));
    }
}
