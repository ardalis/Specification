using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public class OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T>
    {
        private readonly Specification<T> specification;

        public OrderedSpecificationBuilder(Specification<T> specification)
        {
            this.specification = specification;
        }

        public IOrderedSpecificationBuilder<T> ThenBy(Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderTypeEnum OrderType)>)specification.OrderExpressions).Add((orderExpression, OrderTypeEnum.ThenBy));
            return this;
        }

        public IOrderedSpecificationBuilder<T> ThenByDescending(Expression<Func<T, object>> orderExpression)
        {
            ((List<(Expression<Func<T, object>> OrderExpression, OrderTypeEnum OrderType)>)specification.OrderExpressions).Add((orderExpression, OrderTypeEnum.ThenByDescending));
            return this;
        }
    }
}
