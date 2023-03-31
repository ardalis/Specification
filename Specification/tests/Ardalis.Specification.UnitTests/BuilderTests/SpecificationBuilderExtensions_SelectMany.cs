using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
  public class SpecificationBuilderExtensions_SelectMany
  {
    [Fact]
    public void SetsNothing_GivenNoSelectManyExpression()
    {
      var spec = new StoreProductNamesEmptySpec();

      spec.SelectManyExpression.Should().BeNull();
    }

    [Fact]
    public void SetsSelector_GivenSelectManyExpression()
    {
      var spec = new StoreProductNamesSpec();

      spec.SelectManyExpression.Should().NotBeNull();
    }
  }
}
