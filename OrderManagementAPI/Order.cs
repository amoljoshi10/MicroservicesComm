using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementAPI
{
    public class Order
    {
        public IEnumerable<OrderLineItem> Items { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double TotalAmount { get; set; }
    }
}
