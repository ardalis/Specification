namespace Tests.Exceptions;

public class InvalidSearchPatternExceptionTests
{
    private const string DEFAULT_MESSAGE = "Invalid search pattern: " + PATTERN;
    private const string PATTERN = "x";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new InvalidSearchPatternException(PATTERN);

        sut.Should().Throw<InvalidSearchPatternException>(PATTERN)
            .WithMessage(DEFAULT_MESSAGE);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new InvalidSearchPatternException(PATTERN, inner);

        sut.Should().Throw<InvalidSearchPatternException>(PATTERN)
            .WithMessage(DEFAULT_MESSAGE)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
