﻿using System;

namespace OpenWeatherApplication.Utils
{
    public class UnauthorizedApiAccessException : Exception
    {
        public UnauthorizedApiAccessException()
        {
        }

        public UnauthorizedApiAccessException(string message) : base(message)
        {
        }

        public UnauthorizedApiAccessException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
