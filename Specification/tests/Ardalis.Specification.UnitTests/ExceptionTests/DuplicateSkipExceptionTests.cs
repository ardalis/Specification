using System;

namespace Ardalis.Specification.UnitTests;

public class DuplicateSkipExceptionTests
{
    private const string _defaultMessage = "Duplicate use of Skip(). Ensure you don't use Skip() more than once in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action action = () => throw new DuplicateSkipException();

        action.Should().Throw<DuplicateSkipException>().WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action action = () => throw new DuplicateSkipException(inner);

        action.Should().Throw<DuplicateSkipException>().WithMessage(_defaultMessage).WithInnerException<Exception>().WithMessage("test");
    }
}
