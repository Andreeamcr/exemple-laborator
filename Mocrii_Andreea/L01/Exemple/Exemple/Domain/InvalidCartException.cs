﻿using System;
using System.Runtime.Serialization;

namespace Exemple.Domain
{
    [Serializable]
    internal class InvalidCartException : Exception
    {
        public InvalidCartException()
        {
        }

        public InvalidCartException(string? message) : base(message)
        {
        }

        public InvalidCartException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidCartException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}