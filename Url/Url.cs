using System;
using System.Collections.Generic;
using System.Text;

namespace G8G.UrlTools
{
    public class Url
    {
        public event EventHandler<ValidationErrorEventArgs> ValidationError;

        protected void OnValidationError()
        {
            ValidationError?.Invoke(this, new ValidationErrorEventArgs());
        }
    }
}
