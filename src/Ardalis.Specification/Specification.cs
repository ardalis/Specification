using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public abstract class Specification<T, TResult> : Specification<T>, ISpecification<T, TResult>
    {
        protected new virtual ISpecificationBuilder<T, TResult> Query { get; }

        protected Specification() : base()
        {
            this.Query = new SpecificationBuilder<T, TResult>(this);
            base.Query = this.Query;
        }

        public Expression<Func<T, TResult>> Selector { get; private set; }

        protected class SpecificationBuilder<TSource, TSourceResult> : SpecificationBuilder<TSource>, ISpecificationBuilder<TSource, TSourceResult>
        {
            private readonly Specification<TSource, TSourceResult> parent;

            public SpecificationBuilder(Specification<TSource, TSourceResult> parent) : base(parent)
            {
                this.parent = parent;
            }

            public ISpecificationBuilder<TSource> Select(Expression<Func<TSource, TSourceResult>> selector)
            {
                parent.Selector = selector;
                return this;
            }
        }
    }

    public abstract class Specification<T> : ISpecification<T>
    {
        protected virtual ISpecificationBuilder<T> Query { get; set; }

        protected Specification()
        {
            this.Query = new SpecificationBuilder<T>(this);
        }

        public IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; } = new List<Expression<Func<T, bool>>>();
        public IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; } = 
            new List<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)>();
        public IEnumerable<IIncludeAggregator> IncludeAggregators { get; } = new List<IIncludeAggregator>();
        public IEnumerable<string> IncludeStrings { get; } = new List<string>();

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;

        public string? CacheKey { get; protected set; }
        public bool CacheEnabled { get; private set; }


        protected class SpecificationBuilder<TSource> : ISpecificationBuilder<TSource>
        {
            private readonly Specification<TSource> parent;
            private readonly IOrderedSpecificationBuilder<TSource> orderedSpecificationBuilder;

            public SpecificationBuilder(Specification<TSource> parent)
            {
                this.parent = parent;
                this.orderedSpecificationBuilder = new OrderedSpecificationBuilder<TSource>(parent);
            }

            public ISpecificationBuilder<TSource> Where(Expression<Func<TSource, bool>> criteria)
            {
                ((List<Expression<Func<TSource, bool>>>)parent.WhereExpressions).Add(criteria);
                return this;
            }

            public IIncludableSpecificationBuilder<TSource, TProperty> Include<TProperty>(Expression<Func<TSource, TProperty>> includeExpression)
            {
                var aggregator = new IncludeAggregator((includeExpression.Body as MemberExpression)?.Member?.Name);
                var includeBuilder = new IncludableSpecificationBuilder<TSource, TProperty>(aggregator);

                ((List<IIncludeAggregator>)parent.IncludeAggregators).Add(aggregator);
                return includeBuilder;
            }

            public ISpecificationBuilder<TSource> Include(string includeString)
            {
                ((List<string>)parent.IncludeStrings).Add(includeString);
                return this;
            }

            public IOrderedSpecificationBuilder<TSource> OrderBy(Expression<Func<TSource, object>> orderExpression)
            {
                ((List<(Expression<Func<TSource, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderBy));
                return orderedSpecificationBuilder;
            }

            public IOrderedSpecificationBuilder<TSource> OrderByDescending(Expression<Func<TSource, object>> orderExpression)
            {
                ((List<(Expression<Func<TSource, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderByDescending));
                return orderedSpecificationBuilder;
            }

            public ISpecificationBuilder<TSource> Paginate(int skip, int take)
            {
                parent.Skip = skip;
                parent.Take = take;
                parent.IsPagingEnabled = true;
                return this;
            }

            /// <summary>
            /// Must be called after specifying criteria
            /// </summary>
            /// <param name="specificationName"></param>
            /// <param name="args">Any arguments used in defining the specification</param>
            protected void EnableCache(string specificationName, params object[] args)
            {
                Guard.Against.NullOrEmpty(specificationName, nameof(specificationName));
                Guard.Against.NullOrEmpty(parent.WhereExpressions, nameof(parent.WhereExpressions));

                parent.CacheKey = $"{specificationName}-{string.Join("-", args)}";

                parent.CacheEnabled = true;
            }
        }

        protected class OrderedSpecificationBuilder<TSourceOrdered> : IOrderedSpecificationBuilder<TSourceOrdered>
        {
            private readonly Specification<TSourceOrdered> parent;

            public OrderedSpecificationBuilder(Specification<TSourceOrdered> parent)
            {
                this.parent = parent;
            }

            public IOrderedSpecificationBuilder<TSourceOrdered> ThenBy(Expression<Func<TSourceOrdered, object>> orderExpression)
            {
                ((List<(Expression<Func<TSourceOrdered, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderBy));
                return this;
            }

            public IOrderedSpecificationBuilder<TSourceOrdered> ThenByDescending(Expression<Func<TSourceOrdered, object>> orderExpression)
            {
                ((List<(Expression<Func<TSourceOrdered, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderByDescending));
                return this;
            }
        }
    }
}
