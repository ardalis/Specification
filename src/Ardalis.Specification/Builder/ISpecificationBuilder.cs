using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public interface ISpecificationBuilder<TSource>
    {
        ISpecificationBuilder<TSource> Where(Expression<Func<TSource, bool>> criteria);
        ISpecificationBuilder<TSource> Paginate(int skip, int take);
    }
}
