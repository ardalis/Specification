using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ardalis.Specification.Operators
{
    public sealed class NotSpecification<T> : CompositeSpecification<T> where T:class
    {
        public NotSpecification(Specification<T> specification)
        {
            CombineWhereExpressions(specification, new FalseSpecification<T>());
            CombineIncludeAggregators(specification, new FalseSpecification<T>());
            CombineOrderExpressions(specification,new FalseSpecification<T>());
        }

        protected override void CombineWhereExpressions(Specification<T> leftSpec, Specification<T> rightSpec)
        {
            var leftSpecExpr = ReduceExpressions(leftSpec.WhereExpressions.Select(e=>e.Filter).ToList());
            if (leftSpecExpr == null)
                return;
            //replace the parameters
            var parameter = Expression.Parameter(typeof(T));
            var leftVisitor = new ReplaceExpressionVisitor(leftSpecExpr.Parameters[0], parameter);
            var left = leftVisitor.Visit(leftSpecExpr.Body);
            
            //apply '!' on Spec where clauses
            var combinedExpr = Expression.Lambda<Func<T, bool>>(
                Expression.Not(left), parameter);
            
            Query.Where(combinedExpr);
        }
    }
}
