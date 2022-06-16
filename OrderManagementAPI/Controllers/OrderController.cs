using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagementAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger,
                                IOrderService orderService
                                )
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<Order> Get()
        {
           _logger.LogInformation("OrderAPI OrderController Get method started");
            return _orderService.GetOrders();
        }
        [HttpPost]
        public  int Create([FromBody] Order order)
        {
            return _orderService.CreateOrder(order);
        }

    }
}
