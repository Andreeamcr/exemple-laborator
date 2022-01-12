using ShoppingApp.Dto;
using ShoppingApp.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Accomodation.EventProcessor
{
    internal class CartPublishedEventHandler : AbstractEventHandler<CartPublishedEvent>
    {
        public override string[] EventTypes => new string[] { typeof(CartPublishedEvent).Name };

        protected override Task<EventProcessingResult> OnHandleAsync(CartPublishedEvent eventData)
        {
            Console.WriteLine(eventData.ToString());
            return Task.FromResult(EventProcessingResult.Completed);
        }
    }
}
