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
    public class CartsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CartsController> _logger;
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService,
                               IConfiguration configuration,
                               ILogger<CartsController> logger)
        {
            _cartService = cartService;
            _configuration = configuration;
            _logger = logger;
        }
        [Route("carts")]
        [HttpGet]
        public async Task<Cart> List()
        {
            List<CartItemLine> items = null;
            var cartViewModel = new ShoppingCartViewModel();
            var order = new Cart();
            var orderItems = new List<CartItemLine>();

            try
            {

                items = await _cartService.List();

                cartViewModel.Items = items;

                foreach (var item in items)
                {
                    var orderLineItem = new CartItemLine();
                    orderLineItem.ItemId = item.ItemId;
                    orderLineItem.Description = item.Description;
                    orderLineItem.Price = item.Price;
                    orderLineItem.Discount = item.Discount;
                    orderItems.Add(orderLineItem);
                }
                order.Items = orderItems;
                
                //TempData.Keep("CartsViewModel");
                _logger.LogInformation("Carts {CartsCount} served from API", items.Capacity);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Source);
                throw;
            }
            return order;
        }
        [HttpPost]
        [Route("carts/checkout")]
        public async Task<Cart> PlaceOrder([FromBody] Cart cart)
        {
            var cartCheckoutJson = JsonConvert.SerializeObject(cart);
            return await _cartService.PlaceOrder(cartCheckoutJson);
        }
        [HttpPost]
        [Route("carts")]
        public async Task<CartItemLine> AddToCart([FromBody] CartItemLine cartItemLine)
        {
            var cartJson = JsonConvert.SerializeObject(cartItemLine);
            return await _cartService.AddToCart(cartJson);
        }

    }
}
