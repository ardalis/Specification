using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    public static class OrderedBuilderExtensions
    {
        public static IOrderedSpecificationBuilder<T> ThenBy<T>(
            this IOrderedSpecificationBuilder<T> orderedBuilder,
            Expression<Func<T, object?>> orderExpression)
        {
            if (!orderedBuilder.IsChainDiscarded)
            {
                ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenBy));
            }

            return orderedBuilder;
        }

        public static IOrderedSpecificationBuilder<T> ThenBy<T>(
            this IOrderedSpecificationBuilder<T> orderedBuilder,
            Expression<Func<T, object?>> orderExpression,
            bool condition)
        {
            if (condition && !orderedBuilder.IsChainDiscarded)
            {
                ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenBy));
            }
            else
            {
                orderedBuilder.IsChainDiscarded = true;
            }

            return orderedBuilder;
        }

        public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
            this IOrderedSpecificationBuilder<T> orderedBuilder,
            Expression<Func<T, object?>> orderExpression)
        {
            if (!orderedBuilder.IsChainDiscarded)
            {
                ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenByDescending));
            }

            return orderedBuilder;
        }

        public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
            this IOrderedSpecificationBuilder<T> orderedBuilder,
            Expression<Func<T, object?>> orderExpression,
            bool condition)
        {
            if (condition && !orderedBuilder.IsChainDiscarded)
            {
                ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenByDescending));
            }
            else
            {
                orderedBuilder.IsChainDiscarded = true;
            }

            return orderedBuilder;
        }
    }
}
