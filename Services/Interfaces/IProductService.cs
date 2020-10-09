using EcomApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcomApp.Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(Product product);

        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<bool> DeleteProductAscync(int id);
        Task<bool> UpdateProductAsync(Product product);



    }
}
