using System.Linq.Expressions;

namespace Ardalis.Specification.EntityFrameworkCore;

internal class ParameterReplacerVisitor : ExpressionVisitor
{
    private readonly Expression _newExpression;
    private readonly ParameterExpression _oldParameter;

    private ParameterReplacerVisitor(ParameterExpression oldParameter, Expression newExpression)
    {
        _oldParameter = oldParameter;
        _newExpression = newExpression;
    }

    internal static Expression Replace(Expression expression, ParameterExpression oldParameter, Expression newExpression)
      => new ParameterReplacerVisitor(oldParameter, newExpression).Visit(expression);

    protected override Expression VisitParameter(ParameterExpression p)
      => p == _oldParameter ? _newExpression : p;
}
