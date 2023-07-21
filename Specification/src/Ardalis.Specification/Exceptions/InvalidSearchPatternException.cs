using System;

namespace Ardalis.Specification;

public class InvalidSearchPatternException : Exception
{
    private const string _message = "Invalid search pattern: ";

    public InvalidSearchPatternException(string searchPattern)
        : base($"{_message}{searchPattern}")
    {
    }

    public InvalidSearchPatternException(string searchPattern, Exception innerException)
        : base($"{_message}{searchPattern}", innerException)
    {
    }
}
