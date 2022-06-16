using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Models
{
    public class CartItemLine
    {
        public int CartID { get; set; }
        public int ItemId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
    }
}
