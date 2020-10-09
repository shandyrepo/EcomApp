using EcomApp.Contracts;
using EcomApp.Contracts.Request;
using EcomApp.Models;
using EcomApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EcomApp.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost(ApiRoutes.Producs.Create)]
        public async Task<IActionResult> Create(ProductCreateRequest productRequest)
        {

            Product product = new Product
            {
                Price = productRequest.Price,
                ProductName = productRequest.Name

            };

            await _productService.CreateProductAsync(product);
            return Ok();

        }

        [HttpDelete(ApiRoutes.Producs.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            var deleted = await _productService.DeleteProductAscync(id);
            if (!deleted)
                return NoContent();

            return Ok();
        }

        [HttpGet(ApiRoutes.Producs.GetOne)]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id == null || id <= 0)
                return NoContent();

            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet(ApiRoutes.Producs.GetAll)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpPut(ApiRoutes.Producs.Update)]
        public async Task<IActionResult> ChangeProduct(ProductUpdateRequest productNewInfo)
        {
            var product = await _productService.GetProductByIdAsync(productNewInfo.ID);
            if (product != null)
            {
                product.ProductName = productNewInfo.Name;
                product.Price = productNewInfo.Price;
                var updated = _productService.UpdateProductAsync(product);

                return Ok(updated.Result ? "Успешно обновлено" : "Не удалось изменить продукт");
            }
            return NotFound();
        }
    }
}
