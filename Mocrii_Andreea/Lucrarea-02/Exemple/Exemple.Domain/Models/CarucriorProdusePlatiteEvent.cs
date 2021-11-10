using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class CarucriorProdusePlatiteEvent
    {
        public interface ICarucriorProdusePlatiteEvent { }

        public record CarucriorProdusePlatiteScucceededEvent : ICarucriorProdusePlatiteEvent
        {
            public string Csv{ get;}
            public DateTime PublishedDate { get; }

            internal CarucriorProdusePlatiteScucceededEvent(string csv, DateTime publishedDate)
            {
                Csv = csv;
                PublishedDate = publishedDate;
            }
        }

        public record CarucriorProdusePlatiteFaildEvent : ICarucriorProdusePlatiteEvent
        {
            public string Reason { get; }

            internal CarucriorProdusePlatiteFaildEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
