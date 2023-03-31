using System;

namespace Ardalis.Specification
{
  public class ConcurrentSelectorsException : Exception
  {
    private const string message = "Concurrent specification selector transforms defined. Ensure only one of the Select() or SelectMany() transforms is used in the same specification!";

    public ConcurrentSelectorsException()
        : base(message)
    {
    }

    public ConcurrentSelectorsException(Exception innerException)
        : base(message, innerException)
    {
    }
  }
}
