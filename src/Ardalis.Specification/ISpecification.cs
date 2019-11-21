using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    // Keeping both interfaces here for clarity. Will fix it afterward

    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>> Selector { get; set; }
    }

    public interface ISpecification<T>
    {
        bool CacheEnabled { get; }
        string CacheKey { get; }

        Expression<Func<T, bool>> Criteria { get; }
        IEnumerable<Expression<Func<T, object>>> Includes { get; }
        IEnumerable<string> IncludeStrings { get; }

        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}