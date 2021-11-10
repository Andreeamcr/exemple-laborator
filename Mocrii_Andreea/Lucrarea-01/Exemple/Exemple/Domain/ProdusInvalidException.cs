using System;
using System.Runtime.Serialization;

namespace Exemple.Domain
{
    [Serializable]
    internal class ProdusInvalidException : Exception
    {
        public ProdusInvalidException()
        {
        }

        public ProdusInvalidException(string? message) : base(message)
        {
        }

        public ProdusInvalidException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ProdusInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}