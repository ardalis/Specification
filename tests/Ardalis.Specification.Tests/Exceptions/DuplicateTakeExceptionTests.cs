namespace Tests.Exceptions;

public class DuplicateTakeExceptionTests
{
    private const string DEFAULT_MESSAGE = "Duplicate use of Take(). Ensure you don't use Take() more than once in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new DuplicateTakeException();

        sut.Should().Throw<DuplicateTakeException>()
            .WithMessage(DEFAULT_MESSAGE);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new DuplicateTakeException(inner);

        sut.Should().Throw<DuplicateTakeException>()
            .WithMessage(DEFAULT_MESSAGE)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
