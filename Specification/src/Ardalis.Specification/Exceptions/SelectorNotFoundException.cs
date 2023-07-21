using System;

namespace Ardalis.Specification;

public class SelectorNotFoundException : Exception
{
    private const string _message = "The specification must have a selector transform defined. Ensure either Select() or SelectMany() is used in the specification!";

    public SelectorNotFoundException()
        : base(_message)
    {
    }

    public SelectorNotFoundException(Exception innerException)
        : base(_message, innerException)
    {
    }
}
