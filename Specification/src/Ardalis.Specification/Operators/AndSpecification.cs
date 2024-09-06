using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ardalis.Specification.Operators
{
    public sealed class AndSpecification<T> : CompositeSpecification<T> 
    {
        public AndSpecification(Specification<T> leftSpec, Specification<T> rightSpec)
        {
            CombineWhereExpressions(leftSpec, rightSpec);
            CombineIncludeAggregators(leftSpec, rightSpec);
            CombineOrderExpressions(leftSpec,rightSpec);
        }
        protected override void CombineWhereExpressions(Specification<T> leftSpec, Specification<T> rightSpec)
        {
            var leftSpecExpr = ReduceExpressions(leftSpec.WhereExpressions.Select(e=>e.Filter).ToList());
            var rightSpecExpr = ReduceExpressions(rightSpec.WhereExpressions.Select(e=>e.Filter).ToList());
            if (leftSpecExpr == null && rightSpecExpr == null)
                return;
            if (leftSpecExpr == null)
            {
                Query.Where(rightSpecExpr);
                return;
            }
            if (rightSpecExpr == null)
            {
                Query.Where(leftSpecExpr);
                return;
            }
            //replace the parameters
            var parameter = Expression.Parameter(typeof(T));
            var leftVisitor = new ReplaceExpressionVisitor(leftSpecExpr.Parameters[0], parameter);
            var left = leftVisitor.Visit(leftSpecExpr.Body);
            
            var rightVisitor = new ReplaceExpressionVisitor(rightSpecExpr.Parameters[0], parameter);
            var right = rightVisitor.Visit(rightSpecExpr.Body);
            
            //apply '&&' on leftSpec and rightSpec where clauses
            var combinedExpr = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
            
            Query.Where(combinedExpr);
        }
    }
}
