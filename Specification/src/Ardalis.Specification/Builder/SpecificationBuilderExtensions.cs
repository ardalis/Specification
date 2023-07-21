using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification;

public static class SpecificationBuilderExtensions
{
    /// <summary>
    /// Specify a predicate that will be applied to the query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="criteria"></param>
    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, bool>> criteria)
        => Where(specificationBuilder, criteria, true);

    /// <summary>
    /// Specify a predicate that will be applied to the query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="criteria"></param>
    /// <param name="condition">If false, the criteria won't be added.</param>
    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, bool>> criteria,
        bool condition)
    {
        if (condition)
        {
            ((List<WhereExpressionInfo<T>>)specificationBuilder.Specification.WhereExpressions).Add(new WhereExpressionInfo<T>(criteria));
        }

        return specificationBuilder;
    }

    /// <summary>
    /// Specify the query result will be ordered by <paramref name="orderExpression"/> in an ascending order
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="orderExpression"></param>
    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression)
        => OrderBy(specificationBuilder, orderExpression, true);

    /// <summary>
    /// Specify the query result will be ordered by <paramref name="orderExpression"/> in an ascending order
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="orderExpression"></param>
    /// <param name="condition">If false, the expression won't be added. The whole Order chain will be discarded.</param>
    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition)
        {
            ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderBy));
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification, !condition);

        return orderedSpecificationBuilder;
    }

    /// <summary>
    /// Specify the query result will be ordered by <paramref name="orderExpression"/> in a descending order
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="orderExpression"></param>
    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression)
        => OrderByDescending(specificationBuilder, orderExpression, true);

    /// <summary>
    /// Specify the query result will be ordered by <paramref name="orderExpression"/> in a descending order
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="orderExpression"></param>
    /// <param name="condition">If false, the expression won't be added. The whole Order chain will be discarded.</param>
    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition)
        {
            ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderByDescending));
        }

        var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification, !condition);

        return orderedSpecificationBuilder;
    }

    /// <summary>
    /// Specify an include expression.
    /// This information is utilized to build Include function in the query, which ORM tools like Entity Framework use
    /// to include related entities (via navigation properties) in the query result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="includeExpression"></param>
    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, TProperty>> includeExpression) where T : class
        => Include(specificationBuilder, includeExpression, true);

    /// <summary>
    /// Specify an include expression.
    /// This information is utilized to build Include function in the query, which ORM tools like Entity Framework use
    /// to include related entities (via navigation properties) in the query result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="includeExpression"></param>
    /// <param name="condition">If false, the expression won't be added. The whole Include chain will be discarded.</param>
    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, TProperty>> includeExpression,
        bool condition) where T : class
    {
        if (condition)
        {
            var info = new IncludeExpressionInfo(includeExpression, typeof(T), typeof(TProperty));

            ((List<IncludeExpressionInfo>)specificationBuilder.Specification.IncludeExpressions).Add(info);
        }

        var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Specification, !condition);

        return includeBuilder;
    }

    /// <summary>
    /// Specify a collection of navigation properties, as strings, to include in the query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="includeString"></param>
    public static ISpecificationBuilder<T> Include<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string includeString) where T : class
        => Include(specificationBuilder, includeString, true);

    /// <summary>
    /// Specify a collection of navigation properties, as strings, to include in the query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="includeString"></param>
    /// <param name="condition">If false, the include expression won't be added.</param>
    public static ISpecificationBuilder<T> Include<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string includeString,
        bool condition) where T : class
    {
        if (condition)
        {
            ((List<string>)specificationBuilder.Specification.IncludeStrings).Add(includeString);
        }

        return specificationBuilder;
    }

    /// <summary>
    /// Specify a 'SQL LIKE' operations for search purposes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="selector">the property to apply the SQL LIKE against</param>
    /// <param name="searchTerm">the value to use for the SQL LIKE</param>
    /// <param name="searchGroup">the index used to group sets of Selectors and SearchTerms together</param>
    public static ISpecificationBuilder<T> Search<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, string>> selector,
        string searchTerm,
        int searchGroup = 1) where T : class
        => Search(specificationBuilder, selector, searchTerm, true, searchGroup);

    /// <summary>
    /// Specify a 'SQL LIKE' operations for search purposes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="selector">the property to apply the SQL LIKE against</param>
    /// <param name="searchTerm">the value to use for the SQL LIKE</param>
    /// <param name="condition">If false, the expression won't be added.</param>
    /// <param name="searchGroup">the index used to group sets of Selectors and SearchTerms together</param>
    public static ISpecificationBuilder<T> Search<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, string>> selector,
        string searchTerm,
        bool condition,
        int searchGroup = 1) where T : class
    {
        if (condition)
        {
            ((List<SearchExpressionInfo<T>>)specificationBuilder.Specification.SearchCriterias).Add(new SearchExpressionInfo<T>(selector, searchTerm, searchGroup));
        }

        return specificationBuilder;
    }

    /// <summary>
    /// Specify the number of elements to return.
    /// </summary>
    /// <param name="specificationBuilder"></param>
    /// <param name="take">number of elements to take</param>
    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int take)
        => Take(specificationBuilder, take, true);

    /// <summary>
    /// Specify the number of elements to return.
    /// </summary>
    /// <param name="specificationBuilder"></param>
    /// <param name="take">number of elements to take</param>
    /// <param name="condition">If false, the value will be discarded.</param>
    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int take,
        bool condition)
    {
        if (condition)
        {
            if (specificationBuilder.Specification.Take != null) throw new DuplicateTakeException();

            specificationBuilder.Specification.Take = take;
        }

        return specificationBuilder;
    }

    /// <summary>
    /// Specify the number of elements to skip before returning the remaining elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="skip">number of elements to skip</param>
    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int skip)
        => Skip(specificationBuilder, skip, true);

    /// <summary>
    /// Specify the number of elements to skip before returning the remaining elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="skip">number of elements to skip</param>
    /// <param name="condition">If false, the value will be discarded.</param>
    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            if (specificationBuilder.Specification.Skip != null) throw new DuplicateSkipException();

            specificationBuilder.Specification.Skip = skip;
        }

        return specificationBuilder;
    }

    /// <summary>
    /// Specify a transform function to apply to the <typeparamref name="T"/> element 
    /// to produce another <typeparamref name="TResult"/> element.
    /// </summary>
    public static ISpecificationBuilder<T, TResult> Select<T, TResult>(
        this ISpecificationBuilder<T, TResult> specificationBuilder,
        Expression<Func<T, TResult>> selector)
    {
        specificationBuilder.Specification.Selector = selector;

        return specificationBuilder;
    }

    /// <summary>
    /// Specify a transform function to apply to the <typeparamref name="T"/> element 
    /// to produce a flattened sequence of <typeparamref name="TResult"/> elements.
    /// </summary>
    public static ISpecificationBuilder<T, TResult> SelectMany<T, TResult>(
        this ISpecificationBuilder<T, TResult> specificationBuilder,
        Expression<Func<T, IEnumerable<TResult>>> selector)
    {
        specificationBuilder.Specification.SelectorMany = selector;

        return specificationBuilder;
    }

    /// <summary>
    /// Specify a transform function to apply to the result of the query 
    /// and returns the same <typeparamref name="T"/> type
    /// </summary>
    public static ISpecificationBuilder<T> PostProcessingAction<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Func<IEnumerable<T>, IEnumerable<T>> predicate)
    {
        specificationBuilder.Specification.PostProcessingAction = predicate;

        return specificationBuilder;
    }

    /// <summary>
    /// Specify a transform function to apply to the result of the query.
    /// and returns another <typeparamref name="TResult"/> type
    /// </summary>
    public static ISpecificationBuilder<T, TResult> PostProcessingAction<T, TResult>(
        this ISpecificationBuilder<T, TResult> specificationBuilder,
        Func<IEnumerable<TResult>, IEnumerable<TResult>> predicate)
    {
        specificationBuilder.Specification.PostProcessingAction = predicate;

        return specificationBuilder;
    }

    /// <summary>
    /// Must be called after specifying criteria
    /// </summary>
    /// <param name="specificationName"></param>
    /// <param name="args">Any arguments used in defining the specification</param>
    public static ICacheSpecificationBuilder<T> EnableCache<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string specificationName,
        params object[] args) where T : class
        => EnableCache(specificationBuilder, specificationName, true, args);

    /// <summary>
    /// Must be called after specifying criteria
    /// </summary>
    /// <param name="specificationName"></param>
    /// <param name="args">Any arguments used in defining the specification</param>
    /// <param name="condition">If false, the caching won't be enabled.</param>
    public static ICacheSpecificationBuilder<T> EnableCache<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string specificationName,
        bool condition,
        params object[] args) where T : class
    {
        if (condition)
        {
            if (string.IsNullOrEmpty(specificationName))
            {
                throw new ArgumentException($"Required input {specificationName} was null or empty.", specificationName);
            }

            specificationBuilder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";

            specificationBuilder.Specification.CacheEnabled = true;
        }

        var cacheBuilder = new CacheSpecificationBuilder<T>(specificationBuilder.Specification, !condition);

        return cacheBuilder;
    }

    /// <summary>
    /// If the entity instances are modified, this will be detected
    /// by the change tracker.
    /// </summary>
    /// <param name="specificationBuilder"></param>
    public static ISpecificationBuilder<T> AsTracking<T>(
        this ISpecificationBuilder<T> specificationBuilder) where T : class
        => AsTracking(specificationBuilder, true);

    /// <summary>
    /// If the entity instances are modified, this will be detected
    /// by the change tracker.
    /// </summary>
    /// <param name="specificationBuilder"></param>
    /// <param name="condition">If false, the setting will be discarded.</param>
    public static ISpecificationBuilder<T> AsTracking<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.AsNoTracking = false;
            specificationBuilder.Specification.AsNoTrackingWithIdentityResolution = false;
            specificationBuilder.Specification.AsTracking = true;
        }

        return specificationBuilder;
    }

    /// <summary>
    /// If the entity instances are modified, this will not be detected
    /// by the change tracker.
    /// </summary>
    /// <param name="specificationBuilder"></param>
    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> specificationBuilder) where T : class
        => AsNoTracking(specificationBuilder, true);

    /// <summary>
    /// If the entity instances are modified, this will not be detected
    /// by the change tracker.
    /// </summary>
    /// <param name="specificationBuilder"></param>
    /// <param name="condition">If false, the setting will be discarded.</param>
    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.AsTracking = false;
            specificationBuilder.Specification.AsNoTrackingWithIdentityResolution = false;
            specificationBuilder.Specification.AsNoTracking = true;
        }

        return specificationBuilder;
    }

    /// <summary>
    /// The generated sql query will be split into multiple SQL queries
    /// </summary>
    /// <remarks>
    /// This feature was introduced in EF Core 5.0. It only works when using Include
    /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    public static ISpecificationBuilder<T> AsSplitQuery<T>(
        this ISpecificationBuilder<T> specificationBuilder) where T : class
        => AsSplitQuery(specificationBuilder, true);

    /// <summary>
    /// The generated sql query will be split into multiple SQL queries
    /// </summary>
    /// <remarks>
    /// This feature was introduced in EF Core 5.0. It only works when using Include
    /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="condition">If false, the setting will be discarded.</param>
    public static ISpecificationBuilder<T> AsSplitQuery<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.AsSplitQuery = true;
        }

        return specificationBuilder;
    }

    /// <summary>
    /// The query will then keep track of returned instances 
    /// (without tracking them in the normal way) 
    /// and ensure no duplicates are created in the query results
    /// </summary>
    /// <remarks>
    /// for more info: https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> specificationBuilder) where T : class
        => AsNoTrackingWithIdentityResolution(specificationBuilder, true);

    /// <summary>
    /// The query will then keep track of returned instances 
    /// (without tracking them in the normal way) 
    /// and ensure no duplicates are created in the query results
    /// </summary>
    /// <remarks>
    /// for more info: https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="condition">If false, the setting will be discarded.</param>
    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.AsTracking = false;
            specificationBuilder.Specification.AsNoTracking = false;
            specificationBuilder.Specification.AsNoTrackingWithIdentityResolution = true;
        }

        return specificationBuilder;
    }

    /// <summary>
    /// The query will ignore the defined global query filters
    /// </summary>
    /// <remarks>
    /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/filters
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
        this ISpecificationBuilder<T> specificationBuilder) where T : class
        => IgnoreQueryFilters(specificationBuilder, true);

    /// <summary>
    /// The query will ignore the defined global query filters
    /// </summary>
    /// <remarks>
    /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/filters
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="specificationBuilder"></param>
    /// <param name="condition">If false, the setting will be discarded.</param>
    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        bool condition) where T : class
    {
        if (condition)
        {
            specificationBuilder.Specification.IgnoreQueryFilters = true;
        }

        return specificationBuilder;
    }
}
