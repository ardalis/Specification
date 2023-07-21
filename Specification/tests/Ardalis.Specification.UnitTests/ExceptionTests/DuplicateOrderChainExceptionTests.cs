using System;

namespace Ardalis.Specification.UnitTests;

public class DuplicateOrderChainExceptionTests
{
    private const string _defaultMessage = "The specification contains more than one Order chain!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action action = () => throw new DuplicateOrderChainException();

        action.Should().Throw<DuplicateOrderChainException>().WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action action = () => throw new DuplicateOrderChainException(inner);

        action.Should().Throw<DuplicateOrderChainException>().WithMessage(_defaultMessage).WithInnerException<Exception>().WithMessage("test");
    }
}
