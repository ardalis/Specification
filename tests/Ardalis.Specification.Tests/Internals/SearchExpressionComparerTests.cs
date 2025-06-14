namespace Tests.Internals;

public class SearchExpressionComparerTests
{
    public record Customer(int Id, string Name);

    private static SearchExpressionInfo<Customer> Create(int group) =>
        new(x => x.Name, $"term{group}", group);

    [Fact]
    public void Compare_ReturnsZero_WhenSameReference()
    {
        var info = Create(1);
        var comparer = SearchExpressionComparer<Customer>.Default;
        comparer.Compare(info, info).Should().Be(0);
    }

    [Fact]
    public void Compare_ReturnsNegative_WhenXIsNull()
    {
        var y = Create(1);
        var comparer = SearchExpressionComparer<Customer>.Default;
        comparer.Compare(null, y).Should().BeLessThan(0);
    }

    [Fact]
    public void Compare_ReturnsPositive_WhenYIsNull()
    {
        var x = Create(1);
        var comparer = SearchExpressionComparer<Customer>.Default;
        comparer.Compare(x, null).Should().BeGreaterThan(0);
    }

    [Fact]
    public void Compare_ReturnsNegative_WhenXGroupLessThanYGroup()
    {
        var x = Create(1);
        var y = Create(2);
        var comparer = SearchExpressionComparer<Customer>.Default;
        comparer.Compare(x, y).Should().BeLessThan(0);
    }

    [Fact]
    public void Compare_ReturnsPositive_WhenXGroupGreaterThanYGroup()
    {
        var x = Create(3);
        var y = Create(2);
        var comparer = SearchExpressionComparer<Customer>.Default;
        comparer.Compare(x, y).Should().BeGreaterThan(0);
    }

    [Fact]
    public void Compare_ReturnsZero_WhenGroupsAreEqualButDifferentInstances()
    {
        var x = Create(2);
        var y = Create(2);
        var comparer = SearchExpressionComparer<Customer>.Default;
        comparer.Compare(x, y).Should().Be(0);
    }
}
