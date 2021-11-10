using System;
using System.Runtime.Serialization;

namespace Exemple.Domain.Models
{
    [Serializable]
    internal class InvalidProdusException : Exception
    {
        public InvalidProdusException()
        {
        }

        public InvalidProdusException(string? message) : base(message)
        {
        }

        public InvalidProdusException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidProdusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}