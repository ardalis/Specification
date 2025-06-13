using System.Runtime.InteropServices;

namespace Ardalis.Specification.EntityFrameworkCore;

public class SearchEvaluator : IEvaluator
{
    private SearchEvaluator() { }
    public static SearchEvaluator Instance { get; } = new SearchEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManySearchExpressions.IsEmpty) return query;

            if (spec.OneOrManySearchExpressions.SingleOrDefault is { } searchExpression)
            {
                return query.ApplySingleLike(searchExpression);
            }

            if (spec.OneOrManySearchExpressions.Values is List<SearchExpressionInfo<T>> list)
            {
                var span = CollectionsMarshal.AsSpan(list);
                return ApplyLike(query, span);
            }
        }


        return query;
    }

    private static IQueryable<T> ApplyLike<T>(IQueryable<T> source, ReadOnlySpan<SearchExpressionInfo<T>> span) where T : class
    {
        var groupStart = 0;
        for (var i = 1; i <= span.Length; i++)
        {
            // If we reached the end of the span or the group has changed, we slice and process the group.
            if (i == span.Length || span[i].SearchGroup != span[groupStart].SearchGroup)
            {
                source = source.ApplyLikesAsOrGroup(span[groupStart..i]);
                groupStart = i;
            }
        }
        return source;
    }
}
