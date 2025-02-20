namespace Tests.Builders;

public class Builder_IgnoreQueryFilters
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoIgnoreQueryFilters()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.IgnoreQueryFilters.Should().Be(false);
        spec2.IgnoreQueryFilters.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenIgnoreQueryFiltersWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .IgnoreQueryFilters(false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .IgnoreQueryFilters(false);

        spec1.IgnoreQueryFilters.Should().Be(false);
        spec2.IgnoreQueryFilters.Should().Be(false);
    }

    [Fact]
    public void SetsIgnoreQueryFilters_GivenIgnoreQueryFilters()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .IgnoreQueryFilters();

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .IgnoreQueryFilters();

        spec1.IgnoreQueryFilters.Should().Be(true);
        spec2.IgnoreQueryFilters.Should().Be(true);
    }
}
