using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Exceptions
{
    public class TodoListValidationException : Exception
    {
        public TodoListValidationException() : base() { }
        public TodoListValidationException(string message) : base(message) { }
        public TodoListValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
