using Newtonsoft.Json;
using OnlineShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Services
{
    public class ProductService :IProductService
    {
        private readonly HttpClient client;

        public ProductService(HttpClient client)
        {
            this.client = client;
           //this.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "5db4e7423b5f45238df44d2ac524baf7");
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