namespace Ardalis.Specification;

internal sealed class SearchExpressionComparer<T> : IComparer<SearchExpressionInfo<T>>
{
    public static readonly SearchExpressionComparer<T> Default = new();
    private SearchExpressionComparer() { }

    public int Compare(SearchExpressionInfo<T>? x, SearchExpressionInfo<T>? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        return x.SearchGroup.CompareTo(y.SearchGroup);
    }
}
