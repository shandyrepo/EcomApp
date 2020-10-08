﻿using EcomApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcomApp.Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(Product product);

        Task<Product> GetProductByIdAsync(int? id);
        Task<List<Product>> GetAllProductsAsync();
        Task<bool> DeleteProductAscync(int? id);

        Task<bool> UpdateProductAsync(int? id);
        


    }
}