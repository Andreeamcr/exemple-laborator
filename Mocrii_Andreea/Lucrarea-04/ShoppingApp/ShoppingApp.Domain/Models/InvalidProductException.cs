using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Domain.Models
{

    [Serializable]
    internal class InvalidProductException : Exception
    {
        public InvalidProductException()
        {
        }

        public InvalidProductException(string? message) : base(message)
        {
        }

        public InvalidProductException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidProductException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
