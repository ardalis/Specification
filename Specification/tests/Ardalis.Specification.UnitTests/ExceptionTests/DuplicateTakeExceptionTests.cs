using System;

namespace Ardalis.Specification.UnitTests;

public class DuplicateTakeExceptionTests
{
    private const string _defaultMessage = "Duplicate use of Take(). Ensure you don't use Take() more than once in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action action = () => throw new DuplicateTakeException();

        action.Should().Throw<DuplicateTakeException>().WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action action = () => throw new DuplicateTakeException(inner);

        action.Should().Throw<DuplicateTakeException>().WithMessage(_defaultMessage).WithInnerException<Exception>().WithMessage("test");
    }
}
