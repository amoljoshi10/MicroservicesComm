using AutoMapper;
using CartAPI.ServiceBusMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAPI
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CheckoutMessage>().ReverseMap();
        }
    }
}
