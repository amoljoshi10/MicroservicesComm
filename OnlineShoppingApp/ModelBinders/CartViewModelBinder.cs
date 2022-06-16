using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShoppingApp.Models;
using System;
using System.Threading.Tasks;

namespace OnlineShoppingApp.ModelBinders
{
    public class CartViewModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));
            var result = new ShoppingCartViewModel();
            var values = bindingContext.ValueProvider.GetValue("Value");
            if (values.Length == 0)
                return Task.CompletedTask;

            var splitData = values.FirstValue.Split(new char[] { '|' });
            if (splitData.Length >= 2)
            {
                
                //result= new ShoppingCartViewModel
                //{
                //    Carts = Convert.ToInt32(splitData[0]),
                //    Name = splitData[1]
                //};


                bindingContext.Result = ModelBindingResult.Success(result);
            }

            return Task.CompletedTask;
        }
    }
}
