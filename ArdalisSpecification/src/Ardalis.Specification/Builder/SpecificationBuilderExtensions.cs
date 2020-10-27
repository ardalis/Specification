using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public static class SpecificationBuilderExtensions
    {
        public static ISpecificationBuilder<T> Where<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, bool>> criteria)
        {
            ((List<Expression<Func<T, bool>>>)specificationBuilder.Specification.WhereExpressions).Add(criteria);

            return specificationBuilder;
        }

        public static IOrderedSpecificationBuilder<T> OrderBy<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderTypeEnum OrderType)>)specificationBuilder.Specification.OrderExpressions)
                .Add((orderExpression, OrderTypeEnum.OrderBy));

            var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);

            return orderedSpecificationBuilder;
        }

        public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderTypeEnum OrderType)>)specificationBuilder.Specification.OrderExpressions)
                .Add((orderExpression, OrderTypeEnum.OrderByDescending));

            var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);

            return orderedSpecificationBuilder;
        }

        public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, TProperty>> includeExpression)
        {
            var aggregator = new IncludeAggregator((includeExpression.Body as MemberExpression)?.Member?.Name);
            var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Specification, aggregator);

            ((List<IIncludeAggregator>)specificationBuilder.Specification.IncludeAggregators).Add(aggregator);
            return includeBuilder;
        }

        public static ISpecificationBuilder<T> Include<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            string includeString)
        {
            ((List<string>)specificationBuilder.Specification.IncludeStrings).Add(includeString);
            return specificationBuilder;
        }


        public static ISpecificationBuilder<T> Search<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Expression<Func<T, string>> selector,
            string searchTerm,
            int searchGroup = 1)
        {
            ((List<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)>)specificationBuilder.Specification.SearchCriterias)
                .Add((selector, searchTerm, searchGroup));

            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> Take<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            int take)
        {
            if (specificationBuilder.Specification.Take != null) throw new DuplicateTakeException();

            specificationBuilder.Specification.Take = take;
            specificationBuilder.Specification.IsPagingEnabled = true;
            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> Skip<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            int skip)
        {
            if (specificationBuilder.Specification.Skip != null) throw new DuplicateSkipException();

            specificationBuilder.Specification.Skip = skip;
            specificationBuilder.Specification.IsPagingEnabled = true;
            return specificationBuilder;
        }

        [Obsolete]
        public static ISpecificationBuilder<T> Paginate<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            int skip,
            int take)
        {
            specificationBuilder.Skip(skip);
            specificationBuilder.Take(take);

            return specificationBuilder;
        }

        public static ISpecificationBuilder<T> InMemory<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            Func<List<T>, List<T>> predicate)
        {
            specificationBuilder.Specification.InMemory = predicate;

            return specificationBuilder;
        }

        public static ISpecificationBuilder<T, TResult> Select<T, TResult>(
            this ISpecificationBuilder<T, TResult> specificationBuilder,
            Expression<Func<T, TResult>> selector)
        {
            specificationBuilder.Specification.Selector = selector;

            return specificationBuilder;
        }

        public static ISpecificationBuilder<T, TResult> InMemory<T, TResult>(
            this ISpecificationBuilder<T, TResult> specificationBuilder,
            Func<List<TResult>, List<TResult>> predicate)
        {
            specificationBuilder.Specification.InMemory = predicate;

            return specificationBuilder;
        }

        /// <summary>
        /// Must be called after specifying criteria
        /// </summary>
        /// <param name="specificationName"></param>
        /// <param name="args">Any arguments used in defining the specification</param>
        public static ISpecificationBuilder<T> EnableCache<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            string specificationName, params object[] args)
        {
            Guard.Against.NullOrEmpty(specificationName, nameof(specificationName));
            Guard.Against.NullOrEmpty(specificationBuilder.Specification.WhereExpressions, nameof(specificationBuilder.Specification.WhereExpressions));

            specificationBuilder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";

            specificationBuilder.Specification.CacheEnabled = true;

            return specificationBuilder;
        }
    }
}
