using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public class OrderedSpecificationBuilder<TSourceOrdered> : IOrderedSpecificationBuilder<TSourceOrdered>
    {
        private readonly Specification<TSourceOrdered> parent;

        public OrderedSpecificationBuilder(Specification<TSourceOrdered> parent)
        {
            this.parent = parent;
        }

        public IOrderedSpecificationBuilder<TSourceOrdered> ThenBy(Expression<Func<TSourceOrdered, object>> orderExpression)
        {
            ((List<(Expression<Func<TSourceOrdered, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.ThenBy));
            return this;
        }

        public IOrderedSpecificationBuilder<TSourceOrdered> ThenByDescending(Expression<Func<TSourceOrdered, object>> orderExpression)
        {
            ((List<(Expression<Func<TSourceOrdered, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.ThenByDescending));
            return this;
        }
    }
}
