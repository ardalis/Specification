namespace Ardalis.Specification;

public sealed class DuplicateSelectorsException : Exception
{
    private const string MESSAGE = "Duplicate specification selector transforms defined. Ensure only to use one of the Select() overloads in the same specification!";

    public DuplicateSelectorsException()
        : base(MESSAGE)
    {
    }

    public DuplicateSelectorsException(Exception innerException)
        : base(MESSAGE, innerException)
    {
    }
}
