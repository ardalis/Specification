using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>>? Selector { get; }
    }

    public interface ISpecification<T>
    {
        IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; }
        IEnumerable<string> IncludeStrings { get; }
        IEnumerable<IIncludeAggregator> IncludeAggregators { get; }
        IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; }

        int? Take { get; }
        int? Skip { get; }

        [Obsolete]
        bool IsPagingEnabled { get; }

        bool CacheEnabled { get; }
        string? CacheKey { get; }
    }
}