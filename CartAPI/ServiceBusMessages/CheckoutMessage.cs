﻿using CartAPI.Controllers;
using ServiceBusIntegration.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAPI.ServiceBusMessages
{
    public class CheckoutMessage : ServiceBusBaseMessage
    {
        public IEnumerable<CartItemLine> Items { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
    }
}