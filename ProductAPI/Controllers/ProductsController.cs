﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DemoAPI.Interfaces;


namespace DemoAPI.Controllers
{
    [ApiController]
    //[Route("[api/products]")]
    //[Route("api/[controller]")]
    [Route("products")]
    public class ProductsController : ControllerBase
    {


        private IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<Product> Get()
        {
             _logger.LogInformation("ProductAPI  Get method starts");
            return _productService.GetProducts();
        }
        [HttpGet("{productId}")]
        public IActionResult GetById(int productId)
        {


            var product=_productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);

        }
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _productService.AddProduct(product);

            return Created("products/{product.ProductID}", product);
        }

        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(int productId, [FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var productEntity = _productService.GetProductById(productId);
            if (productEntity == null)
            {
                return NotFound();
            }
            
            _productService.UpdateProduct(productEntity, product);

            return NoContent();
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            var productEntity = _productService.GetProductById(productId);
            if (productEntity == null)
            {
                return NotFound();
            }
            _productService.DeletProduct(productEntity);
            return Ok(true);
        }

        [HttpGet("{kvname}/{secret}")]
        public string GetSecretsAndKeys(string kvname,string secret)
        {
            // read your secret from Azure Key Vault
            string kvUri =
                    "https://kv-for-apim.vault.azure.net/";

            SecretClient client =
                new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            return client.GetSecretAsync(secret).Result.Value.Value;
        }


    }
}
