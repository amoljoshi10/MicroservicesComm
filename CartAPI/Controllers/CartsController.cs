using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CartAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceBusIntegration.MessageBus;
using CartAPI.ServiceBusMessages;
using AutoMapper;

namespace CartAPI.Controllers
{
    
    [ApiController]
    public class CartsController : ControllerBase
    {
        private ICartService _cartService;
        private readonly IMessageBusPublisher _messageBusPublisher;
        private readonly IMapper _mapper;
        public CartsController(ICartService cartService,
                               IMapper mapper,
                               IMessageBusPublisher messageBusPublisher)
        {
            _messageBusPublisher = messageBusPublisher;
            _mapper = mapper;
            _cartService = cartService;
        }

        [HttpGet()]
        [Route("carts")]
        public IEnumerable<CartItemLine> Get()
        {
            return _cartService.GetCarts();
        }
        [HttpPost]
        [Route("carts")]
        public IActionResult AddToCart([FromBody] CartItemLine cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _cartService.AddToCart(cart);
            return Created("cart/{cart.CartID}", cart);
        }

        [HttpPost]
        [Route("carts/checkout")]
        public  async Task<IActionResult> Checkout([FromBody] Cart cart)
        {
            CheckoutMessage cartCheckoutMessage;
            try
            {
                cartCheckoutMessage = _mapper.Map<CheckoutMessage>(cart);
                await _messageBusPublisher.PublishMessage(cartCheckoutMessage, "cartcheckoutmessage");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            
            return Accepted(cartCheckoutMessage); 
            
        }
    }
}
