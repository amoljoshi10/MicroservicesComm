using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShoppingApp.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using OnlineShoppingApp.ModelBinders;
using OnlineShoppingApp.Services;

namespace OnlineShoppingApp.Controllers
{
    public class OrdersController : Controller
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
        public async Task<IActionResult> List()
        {
            List<Cart> orders = new List<Cart>();

            try
            {
                orders =await _orderService.List();
                
                _logger.LogInformation("Orders {ProductCount} served from API", orders.Capacity);
                var cachedDataString = JsonConvert.SerializeObject(orders);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Source);
                throw;
            }
            return View(orders);
        }

        

        public IActionResult BuyComplete()
        {
            return View();
        }
    }
}