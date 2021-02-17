using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
    /// <summary>
    /// Evaluates the logic encapsulated by an <see cref="ISpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ISpecification{T}"/> being evaluated.</typeparam>
    public interface ISpecificationEvaluator<T> where T : class
    {
        /// <summary>
        /// Applies the logic encapsulated by <paramref name="specification"/> to given <paramref name="inputQuery"/>,
        /// and projects the result into <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="inputQuery">The sequence of <typeparamref name="T"/></param>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>A filtered sequence of <typeparamref name="TResult"/></returns>
        IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification);
        /// <summary>
        /// Applies the logic encapsulated by <paramref name="specification"/> to given <paramref name="inputQuery"/>.
        /// </summary>
        /// <param name="inputQuery">The sequence of <typeparamref name="T"/></param>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>A filtered sequence of <typeparamref name="T"/></returns>
        IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification);
    }
}
