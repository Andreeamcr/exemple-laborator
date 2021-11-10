using System;
using System.Runtime.Serialization;



namespace Exemple.Domain.Models
{
    [Serializable]
    internal class InvalidAdresaLivrareException : Exception
    {
        public InvalidAdresaLivrareException()
        {
        }

        public InvalidAdresaLivrareException(string? message) : base(message)
        {
        }

        public InvalidAdresaLivrareException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidAdresaLivrareException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
