using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ardalis.Specification.EntityFramework6;

public class OrderEvaluator : IEvaluator, IInMemoryEvaluator
{
    private OrderEvaluator() { }
    public static OrderEvaluator Instance { get; } = new OrderEvaluator();

    public bool IsCriteriaEvaluator { get; } = false;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.OrderExpressions != null)
        {
            if (specification.OrderExpressions.Count(x => x.OrderType == OrderTypeEnum.OrderBy
                    || x.OrderType == OrderTypeEnum.OrderByDescending) > 1)
            {
                throw new DuplicateOrderChainException();
            }

            IOrderedQueryable<T> orderedQuery = null;
            foreach (var orderExpression in specification.OrderExpressions)
            {
                if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
                {
                    orderedQuery = Queryable.OrderBy((dynamic)query, (dynamic)RemoveConvert(orderExpression.KeySelector));
                }
                else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
                {
                    orderedQuery = Queryable.OrderByDescending((dynamic)query, (dynamic)RemoveConvert(orderExpression.KeySelector));
                }
                else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
                {
                    orderedQuery = Queryable.ThenBy((dynamic)orderedQuery, (dynamic)RemoveConvert(orderExpression.KeySelector));
                }
                else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
                {
                    orderedQuery = Queryable.ThenByDescending((dynamic)orderedQuery, (dynamic)RemoveConvert(orderExpression.KeySelector));
                }
            }

            if (orderedQuery != null)
            {
                query = orderedQuery;
            }
        }

        return query;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
    {
        if (specification.OrderExpressions != null)
        {
            if (specification.OrderExpressions.Count(x => x.OrderType == OrderTypeEnum.OrderBy
                    || x.OrderType == OrderTypeEnum.OrderByDescending) > 1)
            {
                throw new DuplicateOrderChainException();
            }

            IOrderedEnumerable<T> orderedQuery = null;
            foreach (var orderExpression in specification.OrderExpressions)
            {
                if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
                {
                    orderedQuery = Queryable.OrderBy((dynamic)query, (dynamic)RemoveConvert(orderExpression.KeySelector));
                }
                else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
                {
                    orderedQuery = Queryable.OrderByDescending((dynamic)query, (dynamic)RemoveConvert(orderExpression.KeySelector));
                }
                else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
                {
                    orderedQuery = Queryable.ThenBy((dynamic)orderedQuery, (dynamic)RemoveConvert(orderExpression.KeySelector));
                }
                else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
                {
                    orderedQuery = Queryable.ThenByDescending((dynamic)orderedQuery, (dynamic)RemoveConvert(orderExpression.KeySelector));
                }
            }

            if (orderedQuery != null)
            {
                query = orderedQuery;
            }
        }

        return query;
    }

    private LambdaExpression RemoveConvert(LambdaExpression source)
    {
        var body = source.Body;
        while (body.NodeType == ExpressionType.Convert)
            body = ((UnaryExpression)body).Operand;

        return Expression.Lambda(body, source.Parameters);
    }
}
