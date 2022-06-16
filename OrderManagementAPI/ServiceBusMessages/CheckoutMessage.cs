using ServiceBusIntegration.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementAPI.ServiceBusMessages
{
    public class CheckoutMessage : ServiceBusBaseMessage
    {
        public Guid UserId { get; set; }


        //payment information
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string CardExpiration { get; set; }


        public double TotalAmount { get; set; }
    }
}
