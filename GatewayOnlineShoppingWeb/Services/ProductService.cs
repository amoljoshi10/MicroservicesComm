using Newtonsoft.Json;
using GatewayOnlineShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GatewayOnlineShoppingWeb.Services
{
    public class ProductService :IProductService
    {
        private readonly HttpClient client;

        public ProductService(HttpClient client)
        {
            this.client = client;
           this.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "18307c78058b4b4cba73b347674439ad");
        }
        public async Task<List<Product>> List()
        {
            List<Product> products = new List<Product>();
            using (var response = await this.client.GetAsync("/products"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
            }
            return products;
        }

        public async Task<Product> Details(int id)
        {
            Product product = null;
            using (var response = await this.client.GetAsync("/products/" + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<Product>(apiResponse);
            }
            return product;
        }

        

    }
}
