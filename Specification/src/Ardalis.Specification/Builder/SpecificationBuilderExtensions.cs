using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
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
        {
            ((List<WhereExpressionInfo<T>>)specificationBuilder.Specification.WhereExpressions).Add(new WhereExpressionInfo<T>(criteria));

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
        {
            ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderBy));

            var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);

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
        {
            ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderByDescending));

            var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);

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
        {
            var info = new IncludeExpressionInfo(includeExpression, typeof(T), typeof(TProperty));

            ((List<IncludeExpressionInfo>)specificationBuilder.Specification.IncludeExpressions).Add(info);

            var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Specification);

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
        {
            ((List<string>)specificationBuilder.Specification.IncludeStrings).Add(includeString);
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
        {
            ((List<SearchExpressionInfo<T>>)specificationBuilder.Specification.SearchCriterias).Add(new SearchExpressionInfo<T>(selector, searchTerm, searchGroup));

            return specificationBuilder;
        }

        /// <summary>
        /// Specify the number of elements to return.
        /// </summary>
        public static ISpecificationBuilder<T> Take<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            int take)
        {
            if (specificationBuilder.Specification.Take != null) throw new DuplicateTakeException();

            specificationBuilder.Specification.Take = take;
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
        {
            if (specificationBuilder.Specification.Skip != null) throw new DuplicateSkipException();

            specificationBuilder.Specification.Skip = skip;
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
            string specificationName, params object[] args) where T : class
        {
            if (string.IsNullOrEmpty(specificationName))
            {
                throw new ArgumentException($"Required input {specificationName} was null or empty.", specificationName);
            }

            specificationBuilder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";

            specificationBuilder.Specification.CacheEnabled = true;

            var cacheBuilder = new CacheSpecificationBuilder<T>(specificationBuilder.Specification);

            return cacheBuilder;
        }

        /// <summary>
        /// If the entity instances are modified, this will not be detected
        /// by the change tracker.
        /// </summary>
        public static ISpecificationBuilder<T> AsNoTracking<T>(
            this ISpecificationBuilder<T> specificationBuilder) where T : class
        {
            specificationBuilder.Specification.AsNoTracking = true;

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
        {
            specificationBuilder.Specification.AsSplitQuery = true;

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
        {
            specificationBuilder.Specification.AsNoTrackingWithIdentityResolution = true;

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
        {
            specificationBuilder.Specification.IgnoreQueryFilters = true;

            return specificationBuilder;
        }
    }
}
