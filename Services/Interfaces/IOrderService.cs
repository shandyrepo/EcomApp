using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcomApp.Models;
using EcomApp.Contracts.Request;

namespace EcomApp.Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync();

        Task<List<Order>> GetOrdersByCustomer();
        Task<List<Product>> GetPopularProductsByUniqueOrders();

        Task<IEnumerable> GetCustomersAbovePriceAsync();
    }
}
