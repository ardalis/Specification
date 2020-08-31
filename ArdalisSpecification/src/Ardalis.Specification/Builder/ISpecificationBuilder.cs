using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public interface ISpecificationBuilder<T, TResult> : ISpecificationBuilder<T>
    {
        ISpecificationBuilder<T> Select(Expression<Func<T, TResult>> selector);
    }

    public interface ISpecificationBuilder<T>
    {
        ISpecificationBuilder<T> Where(Expression<Func<T, bool>> criteria);
        IOrderedSpecificationBuilder<T> OrderBy(Expression<Func<T, object>> orderExpression);
        IOrderedSpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> orderExpression);
        ISpecificationBuilder<T> Include(string includeString);
        IIncludableSpecificationBuilder<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> includeExpression);
        ISpecificationBuilder<T> EnableCache(string specificationName, params object[] args);
        ISpecificationBuilder<T> Take(int take);
        ISpecificationBuilder<T> Skip(int skip);
        
        [Obsolete]
        ISpecificationBuilder<T> Paginate(int skip, int take);
    }
}
