namespace Ardalis.Specification;

/// <summary>
/// Represents an evaluator that processes a specification.
/// </summary>
public interface IEvaluator
{
    /// <summary>
    /// Whether the evaluator should be omitted for pagination purposes.
    /// </summary>
    bool IsCriteriaEvaluator { get; }

    /// <summary>
    /// Evaluates the given specification on the provided queryable source.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="query">The queryable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <returns>The evaluated queryable source.</returns>
    IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class;
}
