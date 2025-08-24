using System.Runtime.InteropServices;

namespace Ardalis.Specification.EntityFramework6;

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

            // The search expressions are already sorted by SearchGroup.
            return ApplyLike(query, spec.OneOrManySearchExpressions.List);
        }


        // We'll never reach this point for our specifications.
        // This is just to cover the case where users have custom ISpecification<T> implementation but use our evaluator.
        // We'll fall back to LINQ for this case.

        foreach (var searchGroup in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
        {
            query = query.ApplyLikesAsOrGroup(searchGroup);
        }

        return query;
    }

    private static IQueryable<T> ApplyLike<T>(IQueryable<T> source, List<SearchExpressionInfo<T>> list) where T : class
    {
        var groupStart = 0;
        for (var i = 1; i <= list.Count; i++)
        {
            // If we reached the end of the span or the group has changed, we slice and process the group.
            if (i == list.Count || list[i].SearchGroup != list[groupStart].SearchGroup)
            {
                source = source.ApplyLikesAsOrGroup(list, groupStart, i);
                groupStart = i;
            }
        }
        return source;
    }
}
