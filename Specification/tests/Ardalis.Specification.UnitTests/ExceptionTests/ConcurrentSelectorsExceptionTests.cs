using System;

namespace Ardalis.Specification.UnitTests;

public class ConcurrentSelectorsExceptionTests
{
    private const string _defaultMessage = "Concurrent specification selector transforms defined. Ensure only one of the Select() or SelectMany() transforms is used in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action action = () => throw new ConcurrentSelectorsException();

        action.Should().Throw<ConcurrentSelectorsException>().WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action action = () => throw new ConcurrentSelectorsException(inner);

        action.Should().Throw<ConcurrentSelectorsException>().WithMessage(_defaultMessage).WithInnerException<Exception>().WithMessage("test");
    }
}
