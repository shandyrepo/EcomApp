using EcomApp.Contracts;
using EcomApp.Contracts.Request;
using EcomApp.Models;
using EcomApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EcomApp.Controllers
{
    /// <summary>
    /// Класс для работы с продуктами
    /// </summary>
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Создание продукта по модели <see cref="EcomApp.Contracts.Request.ProductCreateRequest"/>  из web-запроса  
        /// </summary>
        [HttpPost(ApiRoutes.Producs.Create)]
        public async Task<IActionResult> Create(ProductCreateRequest productRequest)
        {
            if (productRequest == null)
                return NoContent();

            Product product = new Product
            {
                Price = productRequest.Price,
                ProductName = productRequest.Name
            };

            await _productService.CreateProductAsync(product);
            return Ok("Продукт создан");

        }
        /// <summary>
        /// Удаление продукта по id
        /// </summary>
        [HttpDelete(ApiRoutes.Producs.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteProductAscync(id);
            if (!deleted)
                return NoContent();

            return Ok("Продукт удален");
        }
        /// <summary>
        /// Получение продукта по id
        /// </summary>
        [HttpGet(ApiRoutes.Producs.GetOne)]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"Не найден продукт с id {id}");

            return Ok(product);
        }
        /// <summary>
        /// Получение списка всех продуктов
        /// </summary>
        [HttpGet(ApiRoutes.Producs.GetAll)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        /// <summary>
        /// Изменение информации о продукте по модели <see cref="EcomApp.Contracts.Request.ProductUpdateRequest"/>  из web-запроса  
        /// </summary>
        [HttpPut(ApiRoutes.Producs.Update)]
        public async Task<IActionResult> ChangeProduct(ProductUpdateRequest productNewInfo)
        {
            if (productNewInfo == null)
                return NoContent();

            var product = await _productService.GetProductByIdAsync(productNewInfo.ID);
            if (product != null)
            {
                product.ProductName = productNewInfo.Name;
                product.Price = productNewInfo.Price;
                var updated = _productService.UpdateProductAsync(product);

                return Ok(updated.Result ? "Успешно обновлено" : "Не удалось изменить продукт");
            }
            return NotFound($"Продукт с id {productNewInfo.ID} не найден");
        }
    }
}
