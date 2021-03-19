﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    public class OrderEvaluator : IEvaluator, IInMemoryEvaluator
    {
        private OrderEvaluator() { }
        public static OrderEvaluator Instance { get; } = new OrderEvaluator();

        public bool IsCriteriaEvaluator { get; } = false;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.OrderExpressions != null)
            {
                if (specification.OrderExpressions.Where(x => x.OrderType == OrderTypeEnum.OrderBy ||
                                                            x.OrderType == OrderTypeEnum.OrderByDescending).Count() > 1)
                {
                    throw new DuplicateOrderChainException();
                }

                IOrderedQueryable<T>? orderedQuery = null;
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
                if (specification.OrderExpressions.Where(x => x.OrderType == OrderTypeEnum.OrderBy ||
                                                            x.OrderType == OrderTypeEnum.OrderByDescending).Count() > 1)
                {
                    throw new DuplicateOrderChainException();
                }

                IOrderedEnumerable<T>? orderedQuery = null;
                foreach (var orderExpression in specification.OrderExpressions)
                {
                    if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
                    {
                        orderedQuery = query.OrderBy(orderExpression.KeySelector.Compile());
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
                    {
                        orderedQuery = query.OrderByDescending(orderExpression.KeySelector.Compile());
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
                    {
                        orderedQuery = orderedQuery.ThenBy(orderExpression.KeySelector.Compile());
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
                    {
                        orderedQuery = orderedQuery.ThenByDescending(orderExpression.KeySelector.Compile());
                    }
                }

                if (orderedQuery != null)
                {
                    query = orderedQuery;
                }
            }

            return query;
        }

        //private Expression<Func<T, int>> RemoveConvert<T>(Expression<Func<T, object>> source)
        //{
        //    var body = source.Body;
        //    while (body.NodeType == ExpressionType.Convert)
        //        body = ((UnaryExpression)body).Operand;

        //    //var conversion = Expression.Convert(body, typeof(object));

        //    return Expression.Lambda<Func<T, int>>(body, source.Parameters);
        //}

        private LambdaExpression RemoveConvert(LambdaExpression source)
        {
            var body = source.Body;
            while (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            return Expression.Lambda(body, source.Parameters);
        }
    }
}