namespace Ardalis.Specification;

public class ConcurrentSelectorsException : Exception
{
    private const string MESSAGE = "Concurrent specification selector transforms defined. Ensure only one of the Select() or SelectMany() transforms is used in the same specification!";

    public ConcurrentSelectorsException()
        : base(MESSAGE)
    {
    }

    public ConcurrentSelectorsException(Exception innerException)
        : base(MESSAGE, innerException)
    {
    }
}
