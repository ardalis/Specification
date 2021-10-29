using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class SelectorNotFoundExceptionTests
    {
        private const string defaultMessage = "The specification must have Selector defined.";

        [Fact]
        public void ThrowWithDefaultConstructor()
        {
            Action action = () => throw new SelectorNotFoundException();

            action.Should().Throw<SelectorNotFoundException>().WithMessage(defaultMessage);
        }

        [Fact]
        public void ThrowWithInnerException()
        {
            Exception inner = new Exception("test");
            Action action = () => throw new SelectorNotFoundException(inner);

            action.Should().Throw<SelectorNotFoundException>().WithMessage(defaultMessage).WithInnerException<Exception>().WithMessage("test");
        }
    }
}
