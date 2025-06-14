namespace Ardalis.Specification;

internal static class CollectionExtensions
{
    public static List<T> AsList<T>(this IEnumerable<T> source)
    {
        if (source is List<T> list)
        {
            return list;
        }

        return source.ToList();
    }
}
