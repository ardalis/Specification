namespace Ardalis.Specification;

public class DuplicateSkipException : Exception
{
    private const string MESSAGE = "Duplicate use of Skip(). Ensure you don't use Skip() more than once in the same specification!";

    public DuplicateSkipException()
        : base(MESSAGE)
    {
    }

    public DuplicateSkipException(Exception innerException)
        : base(MESSAGE, innerException)
    {
    }
}
