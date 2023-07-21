using System;

namespace Ardalis.Specification;

public class DuplicateSkipException : Exception
{
    private const string _message = "Duplicate use of Skip(). Ensure you don't use Skip() more than once in the same specification!";

    public DuplicateSkipException()
        : base(_message)
    {
    }

    public DuplicateSkipException(Exception innerException)
        : base(_message, innerException)
    {
    }
}
