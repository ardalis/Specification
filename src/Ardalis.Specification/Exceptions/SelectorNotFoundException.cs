namespace Ardalis.Specification;

public class SelectorNotFoundException : Exception
{
    private const string MESSAGE = "The specification must have a selector transform defined. Ensure either Select() or SelectMany() is used in the specification!";

    public SelectorNotFoundException()
        : base(MESSAGE)
    {
    }

    public SelectorNotFoundException(Exception innerException)
        : base(MESSAGE, innerException)
    {
    }
}
