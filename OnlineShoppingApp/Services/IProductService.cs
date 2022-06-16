using OnlineShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Services
{
    public interface IProductService
    {
        public  Task<List<Product>> List();
        public Task<Product> Details(int id);
    }
}
