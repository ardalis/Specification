using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public interface IOrderedSpecificationBuilder<T>
    {
        IOrderedSpecificationBuilder<T> ThenBy(Expression<Func<T, object>> orderExpression);
        IOrderedSpecificationBuilder<T> ThenByDescending(Expression<Func<T, object>> orderExpression);
    }
}
