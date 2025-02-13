namespace Tests.Builders;

public class Builder_Search
{
    public record Customer(int Id, string FirstName, string LastName);

    [Fact]
    public void DoesNothing_GivenNoSearch()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.SearchCriterias.Should().BeEmpty();
        spec2.SearchCriterias.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenSearchWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Search(x => x.FirstName, "%a%", false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Search(x => x.FirstName, "%a%", false);

        spec1.SearchCriterias.Should().BeEmpty();
        spec2.SearchCriterias.Should().BeEmpty();
    }

    [Fact]
    public void AddsSearch_GivenSingleSearch()
    {
        Expression<Func<Customer, string?>> expr = x => x.FirstName;
        var pattern = "%a%";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Search(expr, pattern);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Search(expr, pattern);

        spec1.SearchCriterias.Should().ContainSingle();
        spec1.SearchCriterias.First().Selector.Should().BeSameAs(expr);
        spec1.SearchCriterias.First().SearchTerm.Should().Be(pattern);
        spec1.SearchCriterias.First().SearchGroup.Should().Be(1);
        spec2.SearchCriterias.Should().ContainSingle();
        spec2.SearchCriterias.First().Selector.Should().BeSameAs(expr);
        spec2.SearchCriterias.First().SearchTerm.Should().Be(pattern);
        spec2.SearchCriterias.First().SearchGroup.Should().Be(1);
    }

    [Fact]
    public void AddsSearch_GivenMultipleSearchInSameGroup()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Search(x => x.FirstName, "%a%")
            .Search(x => x.LastName, "%a%");

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Search(x => x.FirstName, "%a%")
            .Search(x => x.LastName, "%a%");

        spec1.SearchCriterias.Should().HaveCount(2);
        spec1.SearchCriterias.Should().AllSatisfy(x => x.SearchGroup.Should().Be(1));
        spec2.SearchCriterias.Should().HaveCount(2);
        spec2.SearchCriterias.Should().AllSatisfy(x => x.SearchGroup.Should().Be(1));
    }

    [Fact]
    public void AddsSearch_GivenMultipleSearchInDifferentGroups()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Search(x => x.FirstName, "%a%", 1)
            .Search(x => x.LastName, "%a%", 2);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Search(x => x.FirstName, "%a%", 1)
            .Search(x => x.LastName, "%a%", 2);

        spec1.SearchCriterias.Should().HaveCount(2);
        spec1.SearchCriterias.Should().OnlyHaveUniqueItems(x => x.SearchGroup);
        spec2.SearchCriterias.Should().HaveCount(2);
        spec2.SearchCriterias.Should().OnlyHaveUniqueItems(x => x.SearchGroup);
    }
}
