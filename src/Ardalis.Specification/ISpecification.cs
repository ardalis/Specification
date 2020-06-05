using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>> Selector { get; set; }
    }

    public interface ISpecification<T>
    {
        bool CacheEnabled { get; }
        string CacheKey { get; }

        IEnumerable<Expression<Func<T, bool>>> Criterias { get; }
        IEnumerable<Expression<Func<T, object>>> Includes { get; }
        IEnumerable<string> IncludeStrings { get; }

        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> ThenBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        //Expression<Func<T, object>> GroupBy { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}