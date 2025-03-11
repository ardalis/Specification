namespace Ardalis.Specification;

public class InvalidSearchPatternException : Exception
{
    private const string MESSAGE = "Invalid search pattern: ";

    public InvalidSearchPatternException(string searchPattern)
        : base($"{MESSAGE}{searchPattern}")
    {
    }

    public InvalidSearchPatternException(string searchPattern, Exception innerException)
        : base($"{MESSAGE}{searchPattern}", innerException)
    {
    }
}
