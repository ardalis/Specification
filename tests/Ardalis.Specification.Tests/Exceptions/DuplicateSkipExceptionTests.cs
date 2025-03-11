namespace Tests.Exceptions;

public class DuplicateSkipExceptionTests
{
    private const string DEFAULT_MESSAGE = "Duplicate use of Skip(). Ensure you don't use Skip() more than once in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new DuplicateSkipException();

        sut.Should().Throw<DuplicateSkipException>().WithMessage(DEFAULT_MESSAGE);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new DuplicateSkipException(inner);

        sut.Should().Throw<DuplicateSkipException>()
            .WithMessage(DEFAULT_MESSAGE)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
