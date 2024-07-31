using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ardalis.Specification;

namespace Ardalis.Specification.Operators
{
    public abstract class CompositeSpecification<T> : Specification<T> where T : class
    {
        protected abstract void CombineWhereExpressions(Specification<T> spec1, Specification<T> spec2);
        protected virtual void CombineOrderExpressions(Specification<T> spec1, Specification<T> spec2)
        {
            var orderSpecs=spec1.OrderExpressions.ToList();
            orderSpecs.AddRange(spec2.OrderExpressions);
            if(!orderSpecs.Any())
                return;
            var orderByFirst = orderSpecs.First();
            var orderByRest = orderSpecs.Skip(1);
            IOrderedSpecificationBuilder<T> orderQuery = null;
            switch (orderByFirst.OrderType)
            {
                case OrderTypeEnum.OrderBy:
                case OrderTypeEnum.ThenBy:
                    orderQuery = Query.OrderBy(orderByFirst.KeySelector);
                    break;
                case OrderTypeEnum.OrderByDescending:
                case OrderTypeEnum.ThenByDescending:
                    orderQuery = Query.OrderByDescending(orderByFirst.KeySelector);
                    break;
            }
            foreach (var rest in orderByRest)
            {
                switch (rest.OrderType)
                {
                    case OrderTypeEnum.OrderBy:
                    case OrderTypeEnum.ThenBy:
                        orderQuery=orderQuery?.ThenBy(rest.KeySelector);
                        break;
                    case OrderTypeEnum.OrderByDescending:
                    case OrderTypeEnum.ThenByDescending:
                        orderQuery=orderQuery?.ThenByDescending(rest.KeySelector);
                        break;
                }
            }
        }

        protected virtual void CombineIncludeAggregators(Specification<T> leftSpec, Specification<T> rightSpec)
        {
            var includeInfo = ((List<IncludeExpressionInfo>)Query.Specification.IncludeExpressions);
            leftSpec.IncludeExpressions.ToList().ForEach(qAgg=>includeInfo.Add(qAgg));
            leftSpec.IncludeStrings.ToList().ForEach(incStr=>Query.Include(incStr));
            rightSpec.IncludeExpressions.ToList().ForEach(qAgg=>includeInfo.Add(qAgg));
            rightSpec.IncludeStrings.ToList().ForEach(incStr=>Query.Include(incStr));
            
        }

        protected static Expression<Func<T, bool>> ReduceExpressions(List<Expression<Func<T, bool>>> expressions)
        {
            if (expressions.Count() < 2)
                return expressions.FirstOrDefault();
            
            return expressions.Aggregate((expr1, expr2) =>
                  Expression.Lambda<Func<T, bool>>(
                      Expression.AndAlso(expr1.Body, expr2.Body), expr2.Parameters[0]));
        }

        protected class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
        {
            public override Expression Visit(Expression node)
            {
                if (node == oldValue)
                    return newValue;
                return base.Visit(node);
            }
        }
    }
}
