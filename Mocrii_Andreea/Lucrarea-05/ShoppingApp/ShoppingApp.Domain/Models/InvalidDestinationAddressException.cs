using System;
using System.Runtime.Serialization;

namespace ShoppingApp.Domain.Models
{
    internal class InvalidDestinationAddressException : Exception
    {
        public InvalidDestinationAddressException()
        {
        }

        public InvalidDestinationAddressException(string? message) : base(message)
        {
        }

        public InvalidDestinationAddressException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidDestinationAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
