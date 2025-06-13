namespace Ardalis.Specification;

public class SearchValidator : IValidator
{
    private SearchValidator() { }
    public static SearchValidator Instance { get; } = new SearchValidator();

    public bool IsValid<T>(T entity, ISpecification<T> specification)
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManySearchExpressions.IsEmpty) return true;

            if (spec.OneOrManySearchExpressions.SingleOrDefault is { } searchExpression)
            {
                return searchExpression.SelectorFunc(entity)?.Like(searchExpression.SearchTerm) ?? false;
            }

            // The search expressions are already sorted by SearchGroup.
            return IsValid(entity, spec.OneOrManySearchExpressions.List);
        }

        // We'll never reach this point for our specifications.
        // This is just to cover the case where users have custom ISpecification<T> implementation but use our validator.
        // We'll fall back to LINQ for this case.

        foreach (var searchGroup in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
        {
            if (searchGroup.Any(c => c.SelectorFunc(entity)?.Like(c.SearchTerm) ?? false) == false) return false;
        }

        return true;
    }

    // This would be simpler using Span<SearchExpressionInfo<TSource>>
    // but CollectionsMarshal.AsSpan is not available in .NET Standard 2.0
    private static bool IsValid<T>(T entity, List<SearchExpressionInfo<T>> list)
    {
        var groupStart = 0;
        for (var i = 1; i <= list.Count; i++)
        {
            // If we reached the end of the span or the group has changed, we slice and process the group.
            if (i == list.Count || list[i].SearchGroup != list[groupStart].SearchGroup)
            {
                if (IsValidInOrGroup(entity, list, groupStart, i) is false)
                {
                    return false;
                }
                groupStart = i;
            }
        }
        return true;

        static bool IsValidInOrGroup(T sourceItem, List<SearchExpressionInfo<T>> list, int from, int to)
        {
            var validOrGroup = false;
            for (int i = from; i < to; i++)
            {
                if (list[i].SelectorFunc(sourceItem)?.Like(list[i].SearchTerm) ?? false)
                {
                    validOrGroup = true;
                    break;
                }
            }
            return validOrGroup;
        }
    }
}
