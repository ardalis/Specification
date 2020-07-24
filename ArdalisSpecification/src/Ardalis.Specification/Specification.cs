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
        }

        public Expression<Func<T, TResult>>? Selector { get; internal set; }
    }

    public abstract class Specification<T> : ISpecification<T>
    {
        protected virtual ISpecificationBuilder<T> Query { get; }

        protected Specification()
        {
            this.Query = new SpecificationBuilder<T>(this);
        }

        public IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; } = new List<Expression<Func<T, bool>>>();

        public IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; } = 
            new List<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)>();

        public IEnumerable<IIncludeAggregator> IncludeAggregators { get; } = new List<IIncludeAggregator>();

        public IEnumerable<string> IncludeStrings { get; } = new List<string>();

        public int Take { get; internal set; }

        public int Skip { get; internal set; }

        public bool IsPagingEnabled { get; internal set; } = false;

        public string? CacheKey { get; internal set; }

        public bool CacheEnabled { get; internal set; }
    }
}
