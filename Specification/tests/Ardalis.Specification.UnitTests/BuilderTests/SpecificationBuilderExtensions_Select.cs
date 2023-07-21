namespace Ardalis.Specification.UnitTests;

public class SpecificationBuilderExtensions_Select
{
    [Fact]
    public void SetsNothing_GivenNoSelectExpression()
    {
        var spec = new StoreNamesEmptySpec();

        spec.Selector.Should().BeNull();
    }

    [Fact]
    public void SetsSelector_GivenSelectExpression()
    {
        var spec = new StoreNamesSpec();

        spec.Selector.Should().NotBeNull();
    }
}
