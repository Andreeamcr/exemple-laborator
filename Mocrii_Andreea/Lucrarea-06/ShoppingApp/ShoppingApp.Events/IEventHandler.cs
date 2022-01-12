using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudNative.CloudEvents;

namespace ShoppingApp.Events
{
    public interface IEventHandler
    {
        string[] EventTypes { get; }

        Task<EventProcessingResult> HandleAsync(CloudEvent couldEvent);
    }
}
