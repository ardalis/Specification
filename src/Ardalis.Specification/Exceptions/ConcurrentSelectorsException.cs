namespace Ardalis.Specification;

/// <summary>
/// Exception thrown when concurrent selector transforms are defined in a specification.
/// </summary>
public class ConcurrentSelectorsException : Exception
{
    private const string MESSAGE = "Concurrent specification selector transforms defined. Ensure only one of the Select() or SelectMany() transforms is used in the same specification!";

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentSelectorsException"/> class.
    /// </summary>
    public ConcurrentSelectorsException()
        : base(MESSAGE)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConcurrentSelectorsException"/> class with a specified inner exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ConcurrentSelectorsException(Exception innerException)
        : base(MESSAGE, innerException)
    {
    }
}
