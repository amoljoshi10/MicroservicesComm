using GatewayOnlineShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayOnlineShoppingWeb.Services
{
    public interface ICartService
    {
        public Task<CartItemLine> AddToCart(string cartJson);
        public Task<List<CartItemLine>> List();
        public Task<Cart> PlaceOrder(string cartCheckoutJson);
    }
}
