using GatewayOnlineShoppingWeb.Models;
using GatewayOnlineShoppingWeb.Services;
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
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public ProductsController(IProductService productService,
             ICartService cartService,
             IConfiguration configuration,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _cartService = cartService;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<IEnumerable<Product>> List()
        {
            List<Product> products = new List<Product>();
            var apiUrl = _configuration["ProductAPIURI"];

            try
            {
                products = await _productService.List();
                _logger.LogInformation("Products {ProductCount} served from API", products.Capacity);
                var cachedDataString = JsonConvert.SerializeObject(products);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.Source);
                throw;
            }
            return products;
        }
        [HttpGet("{productId}")]
        public async Task<Product> Details(int productId)
        {
            Product product = null;
            var apiUrl = _configuration["ProductAPIURI"];
            product = await _productService.Details(productId);
            return product;
        }
        [HttpGet("{productId}/cart")]
        public async Task<bool> AddToCart(int id)
        {
            Product product = null;
            var apiUrl = _configuration["ProductAPIURI"];

            //Get the product first
            product = await _productService.Details(id);

            var newCart = new CartItemLine();
            newCart.ItemId = product.ProductID;
            newCart.Description = product.ProductDescription;
            newCart.Price = product.Price;
            string newCartJson;
            CartItemLine cart = null;

            try
            {
                newCartJson = JsonConvert.SerializeObject(newCart);
                cart = await _cartService.AddToCart(newCartJson);
            }
            catch (Exception ex)
            {

            }
            return true;
        }
    }
}
