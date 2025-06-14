﻿namespace Ardalis.Specification;

/// <summary>
/// Represents a specification with a selector for projecting entities of type <typeparamref name="T"/> to <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type being queried against.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ISpecification<T, TResult> : ISpecification<T>
{
    /// <summary>
    /// Gets the specification builder.
    /// </summary>
    new ISpecificationBuilder<T, TResult> Query { get; }

    /// <summary>
    /// The Select transform function to apply to the <typeparamref name="T"/> element.
    /// </summary>
    Expression<Func<T, TResult>>? Selector { get; }

    /// <summary>
    /// The SelectMany transform function to apply to the <typeparamref name="T"/> element.
    /// </summary>
    Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; }

    /// <summary>
    /// The transform function to apply to the result of the query encapsulated by the <see cref="ISpecification{T, TResult}"/>.
    /// </summary>
    new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; }

    /// <summary>
    /// Applies the specification to the given entities and returns the filtered results.
    /// </summary>
    /// <param name="entities">The entities to evaluate.</param>
    /// <returns>The filtered results after applying the specification.</returns>
    new IEnumerable<TResult> Evaluate(IEnumerable<T> entities);
}

/// <summary>
/// Represents a specification for querying entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type being queried against.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the specification builder.
    /// </summary>
    ISpecificationBuilder<T> Query { get; }

    /// <summary>
    /// Arbitrary state to be accessed from builders and evaluators.
    /// </summary>
    Dictionary<string, object> Items { get; }

    /// <summary>
    /// The collection of filters.
    /// </summary>
    IEnumerable<WhereExpressionInfo<T>> WhereExpressions { get; }

    /// <summary>
    /// The collections of functions used to determine the sorting (and subsequent sorting),
    /// to apply to the result of the query encapsulated by the <see cref="ISpecification{T}"/>.
    /// </summary>
    IEnumerable<OrderExpressionInfo<T>> OrderExpressions { get; }

    /// <summary>
    /// The collection of <see cref="IncludeExpressionInfo"/>s describing each include expression.
    /// This information is utilized to build Include/ThenInclude functions in the query.
    /// </summary>
    IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; }

    /// <summary>
    /// The collection of navigation properties, as strings, to include in the query.
    /// </summary>
    IEnumerable<string> IncludeStrings { get; }

    /// <summary>
    /// The collection of 'SQL LIKE' operations.
    /// </summary>
    IEnumerable<SearchExpressionInfo<T>> SearchCriterias { get; }

    /// <summary>
    /// The number of elements to return.
    /// </summary>
    int Take { get; }

    /// <summary>
    /// The number of elements to skip before returning the remaining elements.
    /// </summary>
    int Skip { get; }

    /// <summary>
    /// The transform function to apply to the result of the query encapsulated by the <see cref="ISpecification{T}"/>.
    /// </summary>
    Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; }

    /// <summary>
    /// Query tags to help correlate specification with generated SQL queries captured in logs.
    /// </summary>
    IEnumerable<string> QueryTags { get; }

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
    /// that are returned.
    /// </summary>
    bool AsTracking { get; }

    /// <summary>
    /// Returns whether or not the change tracker will track any of the entities
    /// that are returned. When true, if the entity instances are modified, this will not be detected
    /// by the change tracker.
    /// </summary>
    bool AsNoTracking { get; }

    /// <summary>
    /// Returns whether or not the generated sql query should be split into multiple SQL queries
    /// </summary>
    /// <remarks>
    /// This feature was introduced in EF Core 5.0. It only works when using Include
    /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
    /// </remarks>
    bool AsSplitQuery { get; }

    /// <summary>
    /// Returns whether or not the query will then keep track of returned instances 
    /// (without tracking them in the normal way) 
    /// and ensure no duplicates are created in the query results
    /// </summary>
    /// <remarks>
    /// for more info: https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries
    /// </remarks>
    bool AsNoTrackingWithIdentityResolution { get; }

    /// <summary>
    /// Returns whether or not the query should ignore the defined global query filters 
    /// </summary>
    /// <remarks>
    /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/filters
    /// </remarks>
    bool IgnoreQueryFilters { get; }

    /// <summary>
    /// Returns whether or not the query should ignore the defined AutoInclude configurations. 
    /// </summary>
    bool IgnoreAutoIncludes { get; }

    /// <summary>
    /// Applies the specification to the given entities and returns the filtered results.
    /// </summary>
    /// <param name="entities">The entities to evaluate.</param>
    /// <returns>The filtered results after applying the specification.</returns>
    IEnumerable<T> Evaluate(IEnumerable<T> entities);

    /// <summary>
    /// Determines whether the given entity satisfies the specification.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns><c>true</c> if the entity satisfies the specification; otherwise, <c>false</c>.</returns>
    bool IsSatisfiedBy(T entity);
}
