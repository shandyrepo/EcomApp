using EcomApp.Data;
using EcomApp.Models;
using EcomApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcomApp.Services
{
    /// <summary>
    /// Сервис для работы продуктами в БД
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly DataContext _dataContext;
        public ProductService(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new NullReferenceException();
        }

        /// <summary>
        /// Внесение продукта в БД
        /// </summary>
        public async Task<bool> CreateProductAsync(Product product)
        {
            if (product == null)
                return false;

            Product existingProduct = _dataContext.Products.FirstOrDefault(e => e.ProductName == product.ProductName);

            if (existingProduct != null)
                return false;

            await _dataContext.Products.AddAsync(product);
            int created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }
        /// <summary>
        /// Удаление продукта из БЖ
        /// </summary>
        public async Task<bool> DeleteProductAscync(int id)
        {
            var product = GetProductByIdAsync(id);

            if (product.Result == null)
                return false;
           
            _dataContext.Products.Remove(product.Result);

            int deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }
        /// <summary>
        /// Получение всех продуктов в БД
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dataContext.Products.ToListAsync();
        }
        /// <summary>
        /// Получение продукта из БД по id
        /// </summary>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dataContext.Products.FindAsync(id);
        }
        /// <summary>
        /// Обновление продукта в БД
        /// </summary>
        public async Task<bool> UpdateProductAsync(Product product)
        {
            if (product == null)
                return false;

            Product existingProduct = _dataContext.Products.FirstOrDefault(p => p.ProductName == product.ProductName);
            if (existingProduct != null && existingProduct.Id != product.Id)
                return false;

            _dataContext.Products.Update(product);
            int updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }
    }
}
