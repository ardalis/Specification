using System;

namespace Ardalis.Specification;

public class DuplicateTakeException : Exception
{
    private const string _message = "Duplicate use of Take(). Ensure you don't use Take() more than once in the same specification!";

    public DuplicateTakeException()
        : base(_message)
    {
    }

    public DuplicateTakeException(Exception innerException)
        : base(_message, innerException)
    {
    }
}
