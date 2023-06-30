using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.UnitTests.BuilderTests
{
  public class SpecificationBuilderExtensions_AsTracking
  {
    [Fact]
    public void DoesNothing_GivenSpecWithoutAsTracking()
    {
      var spec = new StoreEmptySpec();

      spec.AsNoTracking.Should().BeNull();
      spec.AsNoTrackingWithIdentityResolution.Should().BeNull();
    }

    [Fact]
    public void DoesNothing_GivenAsTrackingWithFalseCondition()
    {
      var spec = new CompanyByIdWithFalseConditions(1);

      spec.AsNoTracking.Should().BeNull();
      spec.AsNoTrackingWithIdentityResolution.Should().BeNull();
    }

    [Fact]
    public void FlagsAsTracking_GivenSpecWithAsTracking()
    {
      var spec = new CompanyByIdAsTrackedSpec(1);

      spec.AsNoTracking.Should().Be(false);
      spec.AsNoTrackingWithIdentityResolution.Should().Be(false);
    }

    [Fact]
    public void FlagsAsTracking_GivenSpecWithAsNoTrackingAndEndWithAsTracking()
    {
      var spec = new CompanyByIdWithAsNoTrackingAsTrackedSpec(1);

      spec.AsNoTracking.Should().Be(false);
      spec.AsNoTrackingWithIdentityResolution.Should().Be(false);
    }
  }
}
