namespace Ardalis.Specification.UnitTests.BuilderTests;

public class SpecificationBuilderExtensions_AsSplitQuery
{
    [Fact]
    public void DoesNothing_GivenSpecWithoutAsSplitQuery()
    {
        var spec = new StoreEmptySpec();

        spec.AsSplitQuery.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsSplitQueryWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.AsSplitQuery.Should().Be(false);
    }

    [Fact]
    public void FlagsAsNoTracking_GivenSpecWithAsSplitQuery()
    {
        var spec = new CompanyByIdAsSplitQuery(1);

        spec.AsSplitQuery.Should().Be(true);
    }
}
