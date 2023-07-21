using System;

namespace Ardalis.Specification.UnitTests;

public class InvalidSearchPatternExceptionTests
{
    private const string _defaultMessage = "Invalid search pattern: " + _pattern;
    private const string _pattern = "x";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action action = () => throw new InvalidSearchPatternException(_pattern);

        action.Should().Throw<InvalidSearchPatternException>(_pattern).WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action action = () => throw new InvalidSearchPatternException(_pattern, inner);

        action.Should().Throw<InvalidSearchPatternException>(_pattern).WithMessage(_defaultMessage).WithInnerException<Exception>().WithMessage("test");
    }
}
