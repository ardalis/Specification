namespace Ardalis.Specification.UnitTests.BuilderTests;

public class SpecificationBuilderExtensions_AsTracking
{
    [Fact]
    public void DoesNothing_GivenSpecWithoutAsTracking()
    {
        var spec = new StoreEmptySpec();

        spec.AsTracking.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsTrackingWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.AsTracking.Should().Be(false);
    }

    [Fact]
    public void FlagsAsTracking_GivenSpecWithAsTracking()
    {
        var spec = new CompanyByIdAsTrackedSpec(1);

        spec.AsTracking.Should().Be(true);
    }

    [Fact]
    public void FlagsAsTracking_GivenSpecWithAsNoTrackingAndEndWithAsTracking()
    {
        var spec = new CompanyByIdWithAsNoTrackingAsTrackedSpec(1);

        spec.AsTracking.Should().Be(true);
    }
}
