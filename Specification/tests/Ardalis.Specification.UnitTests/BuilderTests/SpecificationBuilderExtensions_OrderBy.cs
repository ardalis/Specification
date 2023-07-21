namespace Ardalis.Specification.UnitTests;

public class SpecificationBuilderExtensions_OrderBy
{
    [Fact]
    public void AddsNothingToList_GivenNoOrderExpression()
    {
        var spec = new StoreEmptySpec();

        spec.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsNothingToList_GivenOrderExpressionWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsOrderExpressionToListWithOrderByType_GivenOrderByExpression()
    {
        var spec = new StoresOrderedSpecByName();

        spec.OrderExpressions.Should().ContainSingle();
        spec.OrderExpressions.Single().OrderType.Should().Be(OrderTypeEnum.OrderBy);
    }
}
