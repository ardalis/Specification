using System;
using System.Linq.Expressions;

namespace Ardalis.Specification.UnitTests;

public class IncludeExpressionInfoTests
{
    private readonly Expression<Func<Company, Country>> _expr;

    public IncludeExpressionInfoTests()
    {
        _expr = x => x.Country!;
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullForLambdaExpression()
    {
        Action sutAction = () => new IncludeExpressionInfo(null!, typeof(Company), typeof(Country));

        sutAction.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullForEntityType()
    {
        Action sutAction = () => new IncludeExpressionInfo(_expr, null!, typeof(Country));

        sutAction.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullForPropertyType()
    {
        Action sutAction = () => new IncludeExpressionInfo(_expr, typeof(Company), null!);

        sutAction.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullForPreviousPropertyType()
    {
        Action sutAction = () => new IncludeExpressionInfo(_expr, typeof(Company), typeof(Country), null!);

        sutAction.Should()
            .Throw<ArgumentNullException>();
    }
}
