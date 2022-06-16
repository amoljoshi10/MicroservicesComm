using Microsoft.Extensions.Logging;
using OrderManagementAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementAPI.Services
{
    public class OrderService : IOrderService
    {
        public static List<Order> _orders = new List<Order>();
        private readonly ILogger<OrderService> _logger;
        public OrderService(ILogger<OrderService> logger)
        {
            _logger = logger;
        }
        public  int CreateOrder(Order order)
        {
           _logger.LogInformation("OrderAPI  OrderService CreateOrder method started");
            _orders.Add(order);
             _logger.LogInformation("OrderAPI  OrderService CreateOrder method ended");
            return _orders.Count;
        }
        
        public IEnumerable<Order> GetOrders()
        {
            _logger.LogInformation("OrderAPI  OrderService GetOrders method started");
            if(_orders!=null)
                _logger.LogInformation("OrderAPI OrderService _ordersCount  {_ordersCount} ", _orders.Count);
            return _orders;
        }
    }
}
