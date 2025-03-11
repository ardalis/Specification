namespace Ardalis.Specification;

public class DuplicateTakeException : Exception
{
    private const string MESSAGE = "Duplicate use of Take(). Ensure you don't use Take() more than once in the same specification!";

    public DuplicateTakeException()
        : base(MESSAGE)
    {
    }

    public DuplicateTakeException(Exception innerException)
        : base(MESSAGE, innerException)
    {
    }
}
