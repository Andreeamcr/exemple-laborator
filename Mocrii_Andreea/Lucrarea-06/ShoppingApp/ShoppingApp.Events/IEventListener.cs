using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Events
{
    public interface IEventListener
    {
        Task StartAsync(string topicName, string subsciptioName, CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
