using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.Exceptions
{
    public class DuplicateSkipException : Exception
    {
        private const string message = "Duplicate use of the Skip(). Ensure you don't use both Paginate() and Skip() in the same specification!";

        public DuplicateSkipException()
            : base(message)
        {
        }

        public DuplicateSkipException(Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
