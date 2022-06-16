using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementAPI.Interfaces
{
    public interface IOrderService
    {
        public IEnumerable<Order> GetOrders();
        public int CreateOrder(Order order);
    }
}
