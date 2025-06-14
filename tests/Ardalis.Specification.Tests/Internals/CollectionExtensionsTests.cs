namespace Tests.Internals;

public class CollectionExtensionsTests
{
    [Fact]
    public void AsList_ReturnsSameListInstance_WhenSourceIsList()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.AsList();
        result.Should().BeSameAs(list);
    }

    [Fact]
    public void AsList_ReturnsNewList_WhenSourceIsArray()
    {
        int[] array = { 1, 2, 3 };
        var result = array.AsList();
        result.Should().BeOfType<List<int>>();
        result.Should().Equal(array);
    }

    [Fact]
    public void AsList_ReturnsNewList_WhenSourceIsEnumerable()
    {
        IEnumerable<int> enumerable = Enumerable.Range(1, 3).Where(x => x > 0);
        var result = enumerable.AsList();
        result.Should().BeOfType<List<int>>();
        result.Should().Equal(new List<int> { 1, 2, 3 });
    }

    [Fact]
    public void AsList_ReturnsEmptyList_WhenSourceIsEmpty()
    {
        IEnumerable<int> empty = Enumerable.Empty<int>();
        var result = empty.AsList();
        result.Should().BeOfType<List<int>>();
        result.Should().BeEmpty();
    }
}
