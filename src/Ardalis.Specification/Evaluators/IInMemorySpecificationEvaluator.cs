namespace Ardalis.Specification;

/// <summary>
/// Evaluates specifications in memory.
/// </summary>
public interface IInMemorySpecificationEvaluator
{
    /// <summary>
    /// Evaluates the given specification on the provided enumerable source and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The enumerable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <returns>The evaluated enumerable result.</returns>
    IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification);

    /// <summary>
    /// Evaluates the given specification on the provided enumerable source and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The enumerable source.</param>
    /// <param name="specification">The specification to evaluate.</param>
    /// <returns>The evaluated enumerable result.</returns>
    IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification);
}
