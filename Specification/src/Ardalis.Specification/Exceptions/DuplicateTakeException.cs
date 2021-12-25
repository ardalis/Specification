using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    public class DuplicateTakeException : Exception
    {
        private const string message = "Duplicate use of Take(). Ensure you don't use Take() in the same specification!";

        public DuplicateTakeException()
            : base(message)
        {
        }

        public DuplicateTakeException(Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
