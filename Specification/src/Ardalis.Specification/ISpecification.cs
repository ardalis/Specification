﻿using System;
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
        new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; }

        new IEnumerable<TResult> Evaluate(IEnumerable<T> entities);
    }

    /// <summary>
    /// Encapsulates query logic for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type being queried against.</typeparam>
    public interface ISpecification<T>
    {
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
        Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; }

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
        /// Applies the query defined within the specification to the given objects.
        /// This is specially helpful when unit testing specification classes
        /// </summary>
        /// <param name="entities">the list of entities to which the specification will be applied</param>
        /// <returns></returns>
        IEnumerable<T> Evaluate(IEnumerable<T> entities);
    }
}