using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
    public class OrderEvaluator : IEvaluator
    {
        private OrderEvaluator() { }
        public static OrderEvaluator Instance { get; } = new OrderEvaluator();

        public bool IsCriteriaEvaluator { get; } = false;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.OrderExpressions != null && specification.OrderExpressions.Count() > 0)
            {
                IOrderedQueryable<T>? orderedQuery = null;
                foreach (var orderExpression in specification.OrderExpressions)
                {
                    if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
                    {
                        orderedQuery = query.OrderBy(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
                    {
                        orderedQuery = query.OrderByDescending(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
                    {
                        orderedQuery = orderedQuery.ThenBy(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
                    {
                        orderedQuery = orderedQuery.ThenByDescending(orderExpression.KeySelector);
                    }

                    if (orderedQuery != null)
                    {
                        query = orderedQuery;
                    }
                }
            }

            return query;
        }
    }
}
