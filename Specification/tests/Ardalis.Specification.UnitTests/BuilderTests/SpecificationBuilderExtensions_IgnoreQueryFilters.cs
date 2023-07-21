namespace Ardalis.Specification.UnitTests.BuilderTests;

public class SpecificationBuilderExtensions_IgnoreQueryFilters
{
    [Fact]
    public void DoesNothing_GivenSpecWithoutIgnoreQueryFilters()
    {
        var spec = new StoreEmptySpec();

        spec.IgnoreQueryFilters.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenIgnoreQueryFiltersWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.IgnoreQueryFilters.Should().Be(false);
    }

    [Fact]
    public void FlagsIgnoreQueryFilters_GivenSpecWithIgnoreQueryFilters()
    {
        var spec = new CompanyByIdIgnoreQueryFilters(1);

        spec.IgnoreQueryFilters.Should().Be(true);
    }
}
