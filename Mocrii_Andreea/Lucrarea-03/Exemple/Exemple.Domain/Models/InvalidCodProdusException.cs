using System;
using System.Runtime.Serialization;

namespace Exemple.Domain.Models
{
    [Serializable]
    internal class InvalidCodProdusException : Exception
    {
        public InvalidCodProdusException()
        {
        }

        public InvalidCodProdusException(string? message) : base(message)
        {
        }

        public InvalidCodProdusException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidCodProdusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}