namespace Ardalis.Specification.UnitTests;

public class SpecificationBuilderExtensions_Search
{
    [Fact]
    public void AddsNothingToList_GivenNoWhereExpression()
    {
        var spec = new StoreEmptySpec();

        spec.SearchCriterias.Should().BeEmpty();
    }

    [Fact]
    public void AddsNothingToList_GivenSearchExpressionWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.SearchCriterias.Should().BeEmpty();
    }

    [Fact]
    public void AddsOneCriteriaWithDefaultGroupToList_GivenOneSearchExpressionWithNoGroup()
    {
        var spec = new StoreSearchByNameSpec("test");

        spec.SearchCriterias.Should().ContainSingle();
        spec.SearchCriterias.Single().SearchTerm.Should().Be("%test%");
        spec.SearchCriterias.Single().SearchGroup.Should().Be(1);
    }

    [Fact]
    public void AddsTwoCriteriaWithSameGroupToList_GivenTwoSearchExpressionWithNoGroup()
    {
        var spec = new StoreSearchByNameOrCitySpec("test");

        var criterias = spec.SearchCriterias.ToList();

        criterias.Should().HaveCount(2);
        criterias.ForEach(x => x.SearchTerm.Should().Be("%test%"));
        criterias.ForEach(x => x.SearchGroup.Should().Be(1));
    }

    [Fact]
    public void AddsTwoCriteriaWithDifferentGroupToList_GivenTwoSearchExpressionWithDistinctGroup()
    {
        var spec = new StoreSearchByNameAndCitySpec("test");

        var criterias = spec.SearchCriterias.ToList();

        criterias.Should().HaveCount(2);
        criterias.ForEach(x => x.SearchTerm.Should().Be("%test%"));
        criterias[0].SearchGroup.Should().Be(1);
        criterias[1].SearchGroup.Should().Be(2);
    }
}
