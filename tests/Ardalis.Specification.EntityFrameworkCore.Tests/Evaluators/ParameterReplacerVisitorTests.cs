using System.Linq.Expressions;

namespace Tests.Evaluators;

public class ParameterReplacerVisitorTests
{
    [Fact]
    public void ReturnsExpressionWithReplacedParameter()
    {
        Expression<Func<int, decimal, bool>> expected = (y, z) => y == 1;

        Expression<Func<int, decimal, bool>> expression = (x, z) => x == 1;
        var oldParameter = expression.Parameters[0];
        var newExpression = Expression.Parameter(typeof(int), "y");

        var visitor = new ParameterReplacerVisitor(oldParameter, newExpression);
        var result = visitor.Visit(expression);

        result.ToString().Should().Be(expected.ToString());
    }
}
