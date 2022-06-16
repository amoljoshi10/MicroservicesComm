using CartAPI.Controllers;
using CartAPI.Interfaces;
using ServiceBusIntegration.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAPI.Services
{
    public class CartService : ICartService
    {
        public static List<CartItemLine> _carts=new List<CartItemLine>();
        
        public CartItemLine GetCartById(int cartID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CartItemLine> GetCarts()
        {
            return _carts;
        }

               
        public void AddToCart(CartItemLine cart)
        {
            _carts.Add(cart);
        }

        
    }
}
