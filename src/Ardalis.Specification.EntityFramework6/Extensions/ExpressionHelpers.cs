using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Ardalis.Specification.EntityFramework6;

public static class ExpressionHelpers
{
    public const char MEMBER_DELIMITER = '.';

    public static Expression RemoveConvert(this Expression expression)
    {
        while (expression is UnaryExpression unaryExpr &&
            (unaryExpr.NodeType == ExpressionType.Convert || unaryExpr.NodeType == ExpressionType.ConvertChecked))
        {
            expression = unaryExpr.Operand;
        }

        return expression;
    }

    public static LambdaExpression RemoveConvert(this LambdaExpression source)
    {
        var body = source.Body;

        while (body is UnaryExpression unaryExpr &&
            (unaryExpr.NodeType == ExpressionType.Convert || unaryExpr.NodeType == ExpressionType.ConvertChecked))
        {
            body = unaryExpr.Operand;
        }

        return Expression.Lambda(body, source.Parameters);
    }

    public static bool TryParsePath(Expression expression, out string path)
    {
        path = null;
        var expr = expression.RemoveConvert();

        if (expr is MemberExpression memberExpression)
        {
            string name = memberExpression.Member.Name;
            if (!TryParsePath(memberExpression.Expression, out var path2))
            {
                return false;
            }

            path = ((path2 is null) ? name : (path2 + MEMBER_DELIMITER + name));
        }
        else if (expr is MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.Name == "Select" && methodCallExpression.Arguments.Count == 2)
            {
                if (!TryParsePath(methodCallExpression.Arguments[0], out var path3))
                {
                    return false;
                }

                if (path3 is not null && methodCallExpression.Arguments[1] is LambdaExpression lambdaExpression)
                {
                    if (!TryParsePath(lambdaExpression.Body, out var path4))
                    {
                        return false;
                    }

                    if (path4 is not null)
                    {
                        path = path3 + MEMBER_DELIMITER + path4;
                        return true;
                    }
                }
            }

            return false;
        }

        return true;
    }

    [ExcludeFromCodeCoverage]
    // Testing more efficient alternatives.
    private static string ParsePath(LambdaExpression propertySelector)
    {
        var sb = new StringBuilder();
        Expression expr = propertySelector.Body;
        bool needsDot = false;

        while (expr is not null)
        {
            if (expr is MemberExpression memberExpr)
            {
                if (needsDot)
                    sb.Insert(0, '.');

                sb.Insert(0, memberExpr.Member.Name);
                expr = memberExpr.Expression;
                needsDot = true;
            }
            else if (expr is UnaryExpression unaryExpr &&
                (unaryExpr.NodeType == ExpressionType.Convert || unaryExpr.NodeType == ExpressionType.ConvertChecked))
            {
                expr = unaryExpr.Operand;
            }
            else
            {
                break;
            }
        }

        return sb.ToString();
    }
}
