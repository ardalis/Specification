using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
  public class InvalidSearchPatternException : Exception
  {
    private const string message = "Invalid search pattern: ";

    public InvalidSearchPatternException(string searchPattern)
        : base($"{message}{searchPattern}")
    {
    }

    public InvalidSearchPatternException(string searchPattern, Exception innerException)
        : base($"{message}{searchPattern}", innerException)
    {
    }
  }
}
