using Newtonsoft.Json;
using OnlineShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Services
{
    public class OrderService :IOrderService
    {
        private readonly HttpClient client;

        public OrderService(HttpClient client)
        {
            this.client = client;
            //this.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "5db4e7423b5f45238df44d2ac524baf7");
        }
        

        public async Task<List<Cart>> List()
        {
            List<Cart> orders = new List<Cart>();
            using (var response = await this.client.GetAsync("/orders"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<List<Cart>>(apiResponse);
            }
            return orders;
        }
    }
}