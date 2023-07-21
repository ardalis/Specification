using System;

namespace Ardalis.Specification.UnitTests;

public class SelectorNotFoundExceptionTests
{
    private const string _defaultMessage = "The specification must have a selector transform defined. Ensure either Select() or SelectMany() is used in the specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action action = () => throw new SelectorNotFoundException();

        action.Should().Throw<SelectorNotFoundException>().WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action action = () => throw new SelectorNotFoundException(inner);

        action.Should().Throw<SelectorNotFoundException>().WithMessage(_defaultMessage).WithInnerException<Exception>().WithMessage("test");
    }
}
