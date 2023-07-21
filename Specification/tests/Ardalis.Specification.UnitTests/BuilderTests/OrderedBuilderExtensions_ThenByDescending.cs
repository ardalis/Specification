namespace Ardalis.Specification.UnitTests;

public class OrderedBuilderExtensions_ThenByDescending
{
    [Fact]
    public void AppendsOrderExpressionToListWithThenByDescendingType_GivenThenByDescendingExpression()
    {
        var spec = new StoresByCompanyOrderedDescByNameThenByDescIdSpec(1);

        var orderExpressions = spec.OrderExpressions.ToList();

        // The list must have two items, since Then can be applied once the first level is applied.
        orderExpressions.Should().HaveCount(2);

        orderExpressions[1].OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
    }

    [Fact]
    public void AddsNothingToList_GivenDiscardedOrderChain()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsNothingToList_GivenThenByDescendingExpressionWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditionsForInnerChains(1);

        spec.OrderExpressions.Should().HaveCount(2);
        spec.OrderExpressions.First().OrderType.Should().Be(OrderTypeEnum.OrderBy);
        spec.OrderExpressions.Skip(1).First().OrderType.Should().Be(OrderTypeEnum.OrderByDescending);
        spec.OrderExpressions.Where(x => x.OrderType == OrderTypeEnum.ThenByDescending).Should().BeEmpty();
    }
}
