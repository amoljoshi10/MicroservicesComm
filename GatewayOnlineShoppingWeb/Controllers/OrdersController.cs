using GatewayOnlineShoppingWeb.Models;
using GatewayOnlineShoppingWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayOnlineShoppingWeb.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService,
                                IConfiguration configuration,
                                ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<List<Cart>> List()
        {
            List<Cart> orders = new List<Cart>();

            try
            {
                orders = await _orderService.List();
                _logger.LogInformation("Orders {OrdersCount} served from API", orders.Capacity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Source);
                throw;
            }
            return orders;
        }
    }
}
