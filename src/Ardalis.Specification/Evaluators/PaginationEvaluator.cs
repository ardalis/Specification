namespace Ardalis.Specification;

/// <summary>
/// Represents an evaluator for take and skip expressions.
/// </summary>
public class PaginationEvaluator : IEvaluator, IInMemoryEvaluator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="PaginationEvaluator"/> class.
    /// </summary>
    public static PaginationEvaluator Instance { get; } = new PaginationEvaluator();
    private PaginationEvaluator() { }

    /// <inheritdoc/>
    public bool IsCriteriaEvaluator { get; } = false;

    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        // If skip is 0, avoid adding to the IQueryable. It will generate more optimized SQL that way.
        if (specification.Skip > 0)
        {
            query = query.Skip(specification.Skip);
        }

        if (specification.Take >= 0)
        {
            query = query.Take(specification.Take);
        }

        return query;
    }

    /// <inheritdoc/>
    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
    {
        if (specification.Skip > 0)
        {
            query = query.Skip(specification.Skip);
        }

        if (specification.Take >= 0)
        {
            query = query.Take(specification.Take);
        }

        return query;
    }
}
