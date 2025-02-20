using System.Linq;

namespace Ardalis.Specification;

/// <summary>
/// Evaluates the logic encapsulated by an <see cref="ISpecification{T}"/>.
/// </summary>
public interface ISpecificationEvaluator
{
    /// <summary>
    /// Applies the logic encapsulated by <paramref name="specification"/> to given <paramref name="inputQuery"/>,
    /// and projects the result into <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="inputQuery">The sequence of <typeparamref name="T"/></param>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>A filtered sequence of <typeparamref name="TResult"/></returns>
    IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification) where T : class;
    /// <summary>
    /// Applies the logic encapsulated by <paramref name="specification"/> to given <paramref name="inputQuery"/>.
    /// </summary>
    /// <param name="inputQuery">The sequence of <typeparamref name="T"/></param>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>A filtered sequence of <typeparamref name="T"/></returns>
    IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class;
}
