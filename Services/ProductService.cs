using EcomApp.Data;
using EcomApp.Models;
using EcomApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcomApp.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _dataContext;
        public ProductService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> CreateProductAsync(Product product)
        {
            var existingProduct = _dataContext.Products.FirstOrDefault(e => e.ProductName == product.ProductName);

            if (existingProduct != null)
                return false;
            await _dataContext.Products.AddAsync(product);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteProductAscync(int? id)
        {
            var product = GetProductByIdAsync(id);

            if (product == null)
                return false;

            _dataContext.Products.Remove(product.Result);

            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _dataContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int? id)
        {
            return await _dataContext.Products.SingleAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var existingProduct = _dataContext.Products.FirstOrDefault(p => p.ProductName == product.ProductName);
            if (existingProduct != null && existingProduct.Id != product.Id)
                return false;

            _dataContext.Products.Update(product);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }
    }
}
