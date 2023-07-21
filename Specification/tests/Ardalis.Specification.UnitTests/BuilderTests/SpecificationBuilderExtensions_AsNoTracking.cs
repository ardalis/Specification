namespace Ardalis.Specification.UnitTests.BuilderTests;

public class SpecificationBuilderExtensions_AsNoTracking
{
    [Fact]
    public void DoesNothing_GivenSpecWithoutAsNoTracking()
    {
        var spec = new StoreEmptySpec();

        spec.AsNoTracking.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsNoTrackingWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.AsNoTracking.Should().Be(false);
    }

    [Fact]
    public void FlagsAsNoTracking_GivenSpecWithAsNoTracking()
    {
        var spec = new CompanyByIdAsUntrackedSpec(1);

        spec.AsNoTracking.Should().Be(true);
    }

    [Fact]
    public void FlagsAsNoTracking_GivenSpecWithAsTrackingAndEndWithAsNoTracking()
    {
        var spec = new CompanyByIdWithAsTrackingAsUntrackedSpec(1);

        spec.AsNoTracking.Should().Be(true);
    }
}
