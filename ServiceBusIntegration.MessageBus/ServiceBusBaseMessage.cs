using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusIntegration.MessageBus
{
    public class ServiceBusBaseMessage 
    {
        public Guid Id { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
