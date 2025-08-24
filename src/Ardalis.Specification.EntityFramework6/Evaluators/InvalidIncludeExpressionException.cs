namespace Ardalis.Specification.EntityFramework6;

public class InvalidIncludeExpressionException : Exception
{
    private const string MESSAGE = "The Include path expression must refer to a navigation property defined on the type. Use dotted paths for reference navigation properties and the Select operator for collection navigation properties.";

    public InvalidIncludeExpressionException()
        : base(MESSAGE)
    {
    }

    public InvalidIncludeExpressionException(Exception innerException)
        : base(MESSAGE, innerException)
    {
    }
}
