namespace Ardalis.Specification.UnitTests;

public class SpecificationBuilderExtensions_Include
{
    [Fact]
    public void AddsNothingToList_GivenNoIncludeExpression()
    {
        var spec = new StoreEmptySpec();

        spec.IncludeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsNothingToList_GivenIncludeExpressionWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.IncludeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsIncludeExpressionInfoToListWithTypeInclude_GivenIncludeExpression()
    {
        var spec = new StoreIncludeAddressSpec();

        spec.IncludeExpressions.Should().ContainSingle();
        spec.IncludeExpressions.Single().Type.Should().Be(IncludeTypeEnum.Include);
    }
}
