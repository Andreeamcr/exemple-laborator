using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class CaruciorProdusePlatiteEvent
    {
        public interface ICaruciorProdusePlatiteEvent { }

        public record CaruciorProdusePlatiteSucceededEvent : ICaruciorProdusePlatiteEvent
        {
            public string Csv{ get;}
            public DateTime PublishedDate { get; }

            internal CaruciorProdusePlatiteSucceededEvent(string csv, DateTime publishedDate)
            {
                Csv = csv;
                PublishedDate = publishedDate;
            }
        }

        public record CaruciorProdusePlatiteFailedEvent : ICaruciorProdusePlatiteEvent
        {
            public string Reason { get; }

            internal CaruciorProdusePlatiteFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
