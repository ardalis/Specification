namespace Ardalis.Specification;

public class DuplicateOrderChainException : Exception
{
    private const string MESSAGE = "The specification contains more than one Order chain!";

    public DuplicateOrderChainException()
        : base(MESSAGE)
    {
    }

    public DuplicateOrderChainException(Exception innerException)
        : base(MESSAGE, innerException)
    {
    }
}
