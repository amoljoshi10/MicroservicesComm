using System;

namespace OrderManagementAPI
{
    public class OrderLineItem
    {
        public int ItemId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
    }
}
