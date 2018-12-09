using System;
using System.Collections.Generic;
using System.Text;

namespace G8G.UrlTools.Exceptions
{

    [Serializable]
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException() : base("There was a mismatch between input and valid input.") { }
        private ValidationErrorException(string message, Exception inner) : base(message, inner) { }
        protected ValidationErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private ValidationErrorException(string message) : base(message) { }
    }
}
