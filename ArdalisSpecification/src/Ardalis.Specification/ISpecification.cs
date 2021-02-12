using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    /// <summary>
    /// Encapsulates query logic for <typeparamref name="T"/>,
    /// and projects the result into <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="T">The type being queried against.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        /// <summary>
        /// The transform function to apply to the <typeparamref name="T"/> element.
        /// </summary>
        Expression<Func<T, TResult>>? Selector { get; }
        /// <summary>
        /// The transform function to apply to the result of the query encapsulated by the <see cref="ISpecification{T, TResult}"/>.
        /// </summary>
        new Func<List<TResult>, List<TResult>>? InMemory { get; }
    }

    /// <summary>
    /// Encapsulates query logic for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type being queried against.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// The collection of predicates to filter on.
        /// </summary>
        IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; }
        /// <summary>
        /// The collections of functions used to determine the sorting (and subsequent sorting),
        /// to apply to the result of the query encapsulated by the <see cref="ISpecification{T}"/>.
        /// <para>KeySelector, a function to extract a key from an element.</para>
        /// <para>OrderType, whether to (subsequently) sort ascending or descending</para>
        /// </summary>
        IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; }
        /// <summary>
        /// The collection of navigation properties, as <see cref="IIncludeAggregator"/>s, to include in the query.
        /// </summary>
        IEnumerable<IIncludeAggregator> IncludeAggregators { get; }
        /// <summary>
        /// The collection of navigation properties, as strings, to include in the query.
        /// </summary>
        IEnumerable<string> IncludeStrings { get; }
        /// <summary>
        /// The collection of 'SQL LIKE' operations, constructed by;
        /// <list type="bullet">
        ///     <item>Selector, the property to apply the SQL LIKE against.</item>
        ///     <item>SearchTerm, the value to use for the SQL LIKE.</item>
        ///     <item>SearchGroup, the index used to group sets of Selectors and SearchTerms together.</item>
        /// </list>
        /// </summary>
        IEnumerable<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)> SearchCriterias { get; }

        /// <summary>
        /// The number of elements to return.
        /// </summary>
        int? Take { get; }
        /// <summary>
        /// The number of elements to skip before returning the remaining elements.
        /// </summary>
        int? Skip { get; }
        [Obsolete]
        bool IsPagingEnabled { get; }

        /// <summary>
        /// The transform function to apply to the result of the query encapsulated by the <see cref="ISpecification{T}"/>.
        /// </summary>
        Func<List<T>, List<T>>? InMemory { get; }

        /// <summary>
        /// Return whether or not the results should be cached.
        /// </summary>
        bool CacheEnabled { get; }
        /// <summary>
        /// The identifier to use to store and retrieve results from the cache.
        /// </summary>
        string? CacheKey { get; }

        /// <summary>
        /// Returns whether or not the change tracker will track any of the entities
        /// that are returned. When false, if the entity instances are modified, this will not be detected
        /// by the change tracker.
        /// </summary>
        bool AsNoTracking { get; }
    }
}