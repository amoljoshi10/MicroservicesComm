using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementAPI.ServiceBusMessaageConsumers
{
    public interface IServiceBusMessageConsumer
    {
        void Start();
        void Stop();
    }
}
