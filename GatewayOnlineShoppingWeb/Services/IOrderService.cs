using GatewayOnlineShoppingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayOnlineShoppingWeb.Services
{
    public interface IOrderService
    {
        public Task<List<Cart>> List();
    }
}
