using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore;

/// <summary>
/// This evaluator applies EF Core's IgnoreQueryFilters feature to a given query
/// See: https://docs.microsoft.com/en-us/ef/core/querying/filters
/// </summary>
public class IgnoreQueryFiltersEvaluator : IEvaluator
{
    private IgnoreQueryFiltersEvaluator() { }
    public static IgnoreQueryFiltersEvaluator Instance { get; } = new IgnoreQueryFiltersEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.IgnoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }
}
