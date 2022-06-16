using OnlineShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Services
{
    public interface ICartService
    {
        public Task<CartItemLine> AddToCart(CartItemLine cartItemLine);
        public Task<Cart> List();
        public Task<Cart> PlaceOrder(Cart cart);
    }
}