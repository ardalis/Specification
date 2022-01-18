using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
  public class DuplicateTakeExceptionTests
  {
    private const string defaultMessage = "Duplicate use of Take(). Ensure you don't use Take() more than once in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
      Action action = () => throw new DuplicateTakeException();

      action.Should().Throw<DuplicateTakeException>().WithMessage(defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
      Exception inner = new Exception("test");
      Action action = () => throw new DuplicateTakeException(inner);

      action.Should().Throw<DuplicateTakeException>().WithMessage(defaultMessage).WithInnerException<Exception>().WithMessage("test");
    }
  }
}
