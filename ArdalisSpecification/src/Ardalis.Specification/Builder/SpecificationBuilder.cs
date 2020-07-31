using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>, ISpecificationBuilder<T, TResult>
    {
        private readonly Specification<T, TResult> specification;

        public SpecificationBuilder(Specification<T, TResult> specification) : base(specification)
        {
            this.specification = specification;
        }

        public ISpecificationBuilder<T> Select(Expression<Func<T, TResult>> selector)
        {
            specification.Selector = selector;
            return this;
        }
    }

    public class SpecificationBuilder<T> : ISpecificationBuilder<T>
    {
        private readonly Specification<T> specification;
        private readonly IOrderedSpecificationBuilder<T> orderedSpecificationBuilder;

        public SpecificationBuilder(Specification<T> specification)
        {
            this.specification = specification;
            this.orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specification);
        }

        public ISpecificationBuilder<T> Where(Expression<Func<T, bool>> criteria)
        {
            ((List<Expression<Func<T, bool>>>)specification.WhereExpressions).Add(criteria);
            return this;
        }

        public IIncludableSpecificationBuilder<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> includeExpression)
        {
            var aggregator = new IncludeAggregator((includeExpression.Body as MemberExpression)?.Member?.Name);
            var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(aggregator);

            ((List<IIncludeAggregator>)specification.IncludeAggregators).Add(aggregator);
            return includeBuilder;
        }

        public ISpecificationBuilder<T> Include(string includeString)
        {
            ((List<string>)specification.IncludeStrings).Add(includeString);
            return this;
        }

        public IOrderedSpecificationBuilder<T> OrderBy(Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderTypeEnum OrderType)>)specification.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderBy));
            return orderedSpecificationBuilder;
        }

        public IOrderedSpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderTypeEnum OrderType)>)specification.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderByDescending));
            return orderedSpecificationBuilder;
        }

        public ISpecificationBuilder<T> Paginate(int skip, int take)
        {
            specification.Skip = skip;
            specification.Take = take;
            specification.IsPagingEnabled = true;
            return this;
        }

        /// <summary>
        /// Must be called after specifying criteria
        /// </summary>
        /// <param name="specificationName"></param>
        /// <param name="args">Any arguments used in defining the specification</param>
        public ISpecificationBuilder<T> EnableCache(string specificationName, params object[] args)
        {
            Guard.Against.NullOrEmpty(specificationName, nameof(specificationName));
            Guard.Against.NullOrEmpty(specification.WhereExpressions, nameof(specification.WhereExpressions));

            specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";

            specification.CacheEnabled = true;
            return this;
        }
    }
}
