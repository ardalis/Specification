namespace Ardalis.Specification.UnitTests;

public class OrderedBuilderExtensions_ThenBy
{
    [Fact]
    public void AppendOrderExpressionToListWithThenByType_GivenThenByExpression()
    {
        var spec = new StoresByCompanyOrderedDescByNameThenByIdSpec(1);

        var orderExpressions = spec.OrderExpressions.ToList();

        // The list must have two items, since Then can be applied once the first level is applied.
        orderExpressions.Should().HaveCount(2);

        orderExpressions[1].OrderType.Should().Be(OrderTypeEnum.ThenBy);
    }

    [Fact]
    public void AddsNothingToList_GivenDiscardedOrderChain()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsNothingToList_GivenThenByExpressionWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditionsForInnerChains(1);

        spec.OrderExpressions.Should().HaveCount(2);
        spec.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec.OrderExpressions.Skip(1).First().OrderType.Should().Be(OrderTypeEnum.OrderByDescending);
        spec.OrderExpressions.Where(x => x.OrderType == OrderTypeEnum.ThenBy).Should().BeEmpty();
    }
}
