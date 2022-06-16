using Newtonsoft.Json;
using GatewayOnlineShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GatewayOnlineShoppingWeb.Services
{
    public class CartService:ICartService
    {
        private readonly HttpClient client;

        public CartService(HttpClient client)
        {
            this.client = client;
            this.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "18307c78058b4b4cba73b347674439ad");
        }
        public async Task<List<CartItemLine>> List()
        {
            List<CartItemLine> items = null;
            using (var response = await this.client.GetAsync("/carts"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<List<CartItemLine>>(apiResponse);
            }
            return items;
        }
        public async Task<CartItemLine> AddToCart(string cartJson)
        {
            CartItemLine cart = null;
            using (var response = await this.client.PostAsync("/carts", new StringContent(cartJson, Encoding.UTF8, "application/json")))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                cart = JsonConvert.DeserializeObject<CartItemLine>(apiResponse);
            }
            return cart;
        }

        public async Task<Cart> PlaceOrder(string cartCheckoutJson)
        {
            Cart cart = null;
            using (var response = await this.client.PostAsync("carts/checkout", new StringContent(cartCheckoutJson, Encoding.UTF8, "application/json")))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                cart = JsonConvert.DeserializeObject<Cart>(apiResponse);
            }
            return cart;
        }
    }
}
