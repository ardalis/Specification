namespace Tests.Exceptions;

public class DuplicateOrderChainExceptionTests
{
    private const string DEFAULT_MESSAGE = "The specification contains more than one Order chain!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new DuplicateOrderChainException();

        sut.Should().Throw<DuplicateOrderChainException>()
            .WithMessage(DEFAULT_MESSAGE);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new DuplicateOrderChainException(inner);

        sut.Should().Throw<DuplicateOrderChainException>()
            .WithMessage(DEFAULT_MESSAGE)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
