using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcomApp.Models;
using EcomApp.Contracts.Request;
using Microsoft.EntityFrameworkCore.Storage;
namespace EcomApp.Services.Interfaces
{
    public interface IOrderService
    {
        IDbContextTransaction InitTransaction { get; }
        Task<bool> CreateOrderAsync(Order order);

        Task<List<Order>> GetOrdersByCustomer();
        Task<List<Product>> GetPopularProductsByUniqueOrders();
        Task<bool> CreateCustomerAsync(Customer customer);

        Task<IEnumerable> GetCustomersAbovePriceAsync();
    }
}
