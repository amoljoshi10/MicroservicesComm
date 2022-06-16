using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Services;

namespace OnlineShoppingApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsController> _logger;
        private TelemetryClient _telemetryClient;
        private string _user;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductsController(IProductService productService,
            ICartService cartService,
             IConfiguration configuration,
            ILogger<ProductsController> logger,
            TelemetryClient telemetryClient)
        {
            _productService = productService;
            _cartService = cartService;
            _configuration = configuration;
            _logger = logger;
            _telemetryClient = telemetryClient;
            _user = _configuration["User1"];
        }
        public async Task<IActionResult> List()
        {
            List<Product> products = new List<Product>();
            var apiUrl = _configuration["GatewayAPIURI"];
            var cacheKey = "GET_ALL_PRODUCTS";
            var eventTelemetryParentCacheKey = "EventTelemetryParentID";
            var eventTelemetryParentCacheValue = Guid.NewGuid().ToString();


            SendEventTelemetry(apiUrl, "List", "ProductListRequestedFromWebApp", "Products List Requested", eventTelemetryParentCacheValue, _user);
            _logger.LogInformation("In ProductManagmentWeb APP_ProductsController");
            _logger.LogInformation("{Controller}: {CurrentMethodName}):", "Products", "List");
            _logger.LogInformation("Calling URL {DestinationURI})", apiUrl);

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
            return View(products);
        }


        // GET: ProductsController/Details/1
        public async Task<IActionResult> Details(int id)
        {
            Product product = null;
            //var apiUrl = _configuration["ProductAPIURI"];
            var eventTelemetryParentCacheKey = "EventTelemetryParentID";

            product = await _productService.Details(id);
            return View(product);
        }

        // GET: ProductsController/OrderProduct/1
        public async Task<IActionResult> AddToCart(int id)
        {
            Product product = null;
            //var apiUrl = _configuration["ProductAPIURI"];
            
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
                //newCartJson = JsonConvert.SerializeObject(newCart);
                cart = await _cartService.AddToCart(newCart);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("List", "Carts");
        }

        private void SendEventTelemetry(string apiUrl,
                                        string methodName,
                                        string eventName,
                                        string eventDescription,
                                        string eventTelemetryParentID,
                                        string userName)
        {
            var _eventProperties = new Dictionary<string, string>();
            var eventTelemetryParentCacheKey = "EventTelemetryParentID";

            _eventProperties.Add(eventTelemetryParentCacheKey, eventTelemetryParentID);

            _eventProperties.Add("EventTelemetryRootName", "ProductManagementEvents");

            var redisCacheConnection = _configuration["RedisCacheConnection"];

            _eventProperties.Add("redisCacheConnection", redisCacheConnection);

            _eventProperties.Add("AppInsightTelemetryEventSource", "OnlineShoppingApp");
            _eventProperties.Add("Component", "OnlineShoppingApp_ProductsController");
            _eventProperties.Add("TargetURL", apiUrl);
            _eventProperties.Add("MethodName", methodName);
            _eventProperties.Add("EventDescription", eventDescription);
            _eventProperties.Add("User", userName);
            _eventProperties.Add("ApplicationTier", "UI");
            _telemetryClient.TrackEvent(eventName, _eventProperties);
        }
        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/1
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                Product product = null;
                var apiUrl = _configuration["ProductAPIURI"];
                var eventTelemetryParentCacheKey = "EventTelemetryParentID";
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(apiUrl + "/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        product = JsonConvert.DeserializeObject<Product>(apiResponse);
                    }
                }
                return View(product);
            }
            catch
            {
                return View();
            }
        }


        // POST: ProductsController/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Product updatedProduct)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {

                Product product = null;
                var apiUrl = _configuration["ProductAPIURI"];
                var eventTelemetryParentCacheKey = "EventTelemetryParentID";
                string updatedProductJson;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(apiUrl + "/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        product = JsonConvert.DeserializeObject<Product>(apiResponse);
                        product.ProductDescription = updatedProduct.ProductDescription;
                        product.isExpired = updatedProduct.isExpired;
                        product.ExpirationDate = updatedProduct.ExpirationDate;
                        updatedProductJson = JsonConvert.SerializeObject(product);
                    }
                    using (var response = await httpClient.PutAsync(apiUrl + "/" + id, new StringContent(updatedProductJson, Encoding.UTF8, "application/json")))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        product = JsonConvert.DeserializeObject<Product>(apiResponse);
                    }
                }
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private void UpdateEventTelemetryContext(TelemetryContext context, string operationParentId, string operationID, string operationName)
        {
            context.Operation.ParentId = operationParentId;
            context.Operation.Id = operationID;
            context.Operation.Name = operationName;
        }

    }
}