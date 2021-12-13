using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    public static class OrderedBuilderExtensions
    {
        public static IOrderedSpecificationBuilder<T> ThenBy<T>(
            this IOrderedSpecificationBuilder<T> orderedBuilder,
            Expression<Func<T, object?>> orderExpression)
        {
            ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenBy));

            return orderedBuilder;
        }

        public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
            this IOrderedSpecificationBuilder<T> orderedBuilder,
            Expression<Func<T, object?>> orderExpression)
        {
            ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenByDescending));

            return orderedBuilder;
        }
    }
}
