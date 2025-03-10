namespace Ardalis.Specification.EntityFrameworkCore;

/// <summary>
/// This evaluator applies EF Core's IgnoreAutoIncludes to a given query
/// </summary>
public class IgnoreAutoIncludesEvaluator : IEvaluator
{
    private IgnoreAutoIncludesEvaluator() { }
    public static IgnoreAutoIncludesEvaluator Instance { get; } = new IgnoreAutoIncludesEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.IgnoreAutoIncludes)
        {
            query = query.IgnoreAutoIncludes();
        }

        return query;
    }
}
