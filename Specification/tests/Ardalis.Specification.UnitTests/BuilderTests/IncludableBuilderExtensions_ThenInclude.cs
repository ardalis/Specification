using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests;

public class IncludableBuilderExtensions_ThenInclude
{
    [Fact]
    public void AppendIncludeExpressionInfoToListWithTypeThenInclude_GivenThenIncludeExpression()
    {
        var spec = new StoreIncludeCompanyThenCountrySpec();

        var includeExpressions = spec.IncludeExpressions.ToList();

        // The list must have two items, since ThenInclude can be applied once the first level is applied.
        includeExpressions.Should().HaveCount(2);

        includeExpressions[1].Type.Should().Be(IncludeTypeEnum.ThenInclude);
    }

    [Fact]
    public void AddsNothingToList_GivenDiscardedIncludeChain()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.IncludeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsNothingToList_GivenThenIncludeExpressionWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditionsForInnerChains(1);

        spec.IncludeExpressions.Should().HaveCount(1);
        spec.IncludeExpressions.First().Type.Should().Be(IncludeTypeEnum.Include);
        spec.IncludeExpressions.Where(x => x.Type == IncludeTypeEnum.ThenInclude).Should().BeEmpty();
    }

    [Fact]
    public void ThenInclude_Append_IncludeExpressionInfo_With_EnumerablePreviousPropertyType()
    {
        var spec = new StoreIncludeCompanyThenStoresSpec();

        var includeExpressions = spec.IncludeExpressions.ToList();

        includeExpressions.Should().HaveCount(3);

        includeExpressions[2].PreviousPropertyType.Should().Be(typeof(IEnumerable<Store>));
    }
}
