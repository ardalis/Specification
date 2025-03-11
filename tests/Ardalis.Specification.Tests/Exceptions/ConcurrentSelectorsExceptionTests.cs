namespace Tests.Exceptions;

public class ConcurrentSelectorsExceptionTests
{
    private const string DEFAULT_MESSAGE = "Concurrent specification selector transforms defined. Ensure only one of the Select() or SelectMany() transforms is used in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new ConcurrentSelectorsException();

        sut.Should().Throw<ConcurrentSelectorsException>()
            .WithMessage(DEFAULT_MESSAGE);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new ConcurrentSelectorsException(inner);

        sut.Should().Throw<ConcurrentSelectorsException>()
            .WithMessage(DEFAULT_MESSAGE)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
