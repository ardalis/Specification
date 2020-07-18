using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public interface IOrderedSpecificationBuilder<TSource>
    {
        IOrderedSpecificationBuilder<TSource> ThenBy(Expression<Func<TSource, object>> orderExpression);
        IOrderedSpecificationBuilder<TSource> ThenByDescending(Expression<Func<TSource, object>> orderExpression);
    }
}
