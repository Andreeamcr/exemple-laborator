using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Domain.Models
{
    [AsChoice]
    public static partial class PaidCartEvent
    {
        public interface IPaidCartEvent { }

        public record SuccessPaidCartEvent : IPaidCartEvent
        {
            public string Csv { get; }
            public DateTime PublishedDate { get; }

            internal SuccessPaidCartEvent(string csv, DateTime publishedDate)
            {
                Csv = csv;
                PublishedDate = publishedDate;
            }
        }

        public record FailedPaidCartEvent : IPaidCartEvent
        {
            public string Reason { get; }

            internal FailedPaidCartEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
