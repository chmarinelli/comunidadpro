﻿using System;

namespace MiComunidadPro.Common.Exceptions
{
    public class AuthorizationValidationException : Exception
    {
        public AuthorizationValidationException(string message)
            : base(message)
        {
        }

        public AuthorizationValidationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
