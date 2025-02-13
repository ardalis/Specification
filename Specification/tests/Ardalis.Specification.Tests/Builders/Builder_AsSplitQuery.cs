namespace Tests.Builders;

public class Builder_AsSplitQuery
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoAsSplitQuery()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.AsSplitQuery.Should().Be(false);
        spec2.AsSplitQuery.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsSplitQueryWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsSplitQuery(false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .AsSplitQuery(false);

        spec1.AsSplitQuery.Should().Be(false);
        spec2.AsSplitQuery.Should().Be(false);
    }

    [Fact]
    public void SetsAsSplitQuery_GivenAsSplitQuery()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsSplitQuery();

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .AsSplitQuery();

        spec1.AsSplitQuery.Should().Be(true);
        spec2.AsSplitQuery.Should().Be(true);
    }
}
