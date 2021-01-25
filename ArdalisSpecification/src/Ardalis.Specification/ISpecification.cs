using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>>? Selector { get; }
        new Func<List<TResult>, List<TResult>>? InMemory { get; }
    }

    public interface ISpecification<T>
    {
        IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; }
        IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; }
        IEnumerable<IIncludeAggregator> IncludeAggregators { get; }
        IEnumerable<string> IncludeStrings { get; }
        IEnumerable<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)> SearchCriterias { get; }

        int? Take { get; }
        int? Skip { get; }
        [Obsolete]
        bool IsPagingEnabled { get; }

        Func<List<T>, List<T>>? InMemory { get; }

        bool CacheEnabled { get; }
        string? CacheKey { get; }

        bool AsNoTracking { get; }
    }
}