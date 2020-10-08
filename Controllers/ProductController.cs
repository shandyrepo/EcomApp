using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EcomApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EcomApp.Contracts;
using EcomApp.Contracts.Request;
using EcomApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EcomApp.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("/Create")]
        public async Task<IActionResult> Create(ProductCreateRequest productRequest)
        {
          
                Product product = new Product
                {
                    price = productRequest.Price,
                    productName = productRequest.Name

                };
            
                await _productService.CreateProductAsync(product);
                return Ok();

        }
        [HttpDelete("/Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            var deleted = await _productService.DeleteProductAscync(id);
            if (!deleted)
                return NoContent();

            return Ok();
        }

        [HttpGet("/GetALL")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
    }
}
