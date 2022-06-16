using CartAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAPI.Interfaces
{
    public interface ICartService
    {
        IEnumerable<CartItemLine> GetCarts();
        CartItemLine GetCartById(int cartID);
        void AddToCart(CartItemLine cart);
        
    }
}
