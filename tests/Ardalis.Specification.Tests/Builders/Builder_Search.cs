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
    public void AddsSearchSortedByGroup_GivenMultipleSearchInDifferentGroups()
    {
        var firstNameSelector = (Expression<Func<Customer, string?>>)(x => x.FirstName);
        var lastNameSelector = (Expression<Func<Customer, string?>>)(x => x.LastName);

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Search(firstNameSelector, "%a%", 1)
            .Search(lastNameSelector, "%a%", 2)
            .Search(firstNameSelector, "%b%", 1);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Search(firstNameSelector, "%a%", 1)
            .Search(lastNameSelector, "%a%", 2)
            .Search(firstNameSelector, "%b%", 1);

        var expected = new[]
        {
            new SearchExpressionInfo<Customer>(firstNameSelector, "%a%", 1),
            new SearchExpressionInfo<Customer>(firstNameSelector, "%b%", 1),
            new SearchExpressionInfo<Customer>(lastNameSelector, "%a%", 2),
        };

        var actual1 = spec1.SearchCriterias.ToArray();
        var actual2 = spec2.SearchCriterias.ToArray();

        actual1.Should().HaveSameCount(expected);
        actual1[0].Selector.Should().Be(expected[0].Selector);
        actual1[0].SearchTerm.Should().Be(expected[0].SearchTerm);
        actual1[0].SearchGroup.Should().Be(expected[0].SearchGroup);
        actual1[1].Selector.Should().Be(expected[1].Selector);
        actual1[1].SearchTerm.Should().Be(expected[1].SearchTerm);
        actual1[1].SearchGroup.Should().Be(expected[1].SearchGroup);
        actual1[2].Selector.Should().Be(expected[2].Selector);
        actual1[2].SearchTerm.Should().Be(expected[2].SearchTerm);
        actual1[2].SearchGroup.Should().Be(expected[2].SearchGroup);

        actual2.Should().HaveSameCount(expected);
        actual2[0].Selector.Should().Be(expected[0].Selector);
        actual2[0].SearchTerm.Should().Be(expected[0].SearchTerm);
        actual2[0].SearchGroup.Should().Be(expected[0].SearchGroup);
        actual2[1].Selector.Should().Be(expected[1].Selector);
        actual2[1].SearchTerm.Should().Be(expected[1].SearchTerm);
        actual2[1].SearchGroup.Should().Be(expected[1].SearchGroup);
        actual2[2].Selector.Should().Be(expected[2].Selector);
        actual2[2].SearchTerm.Should().Be(expected[2].SearchTerm);
        actual2[2].SearchGroup.Should().Be(expected[2].SearchGroup);

    }
}
