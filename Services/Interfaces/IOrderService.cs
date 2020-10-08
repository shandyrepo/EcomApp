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
        Task<bool> CreateOrderAsync(string email, Order order);

        Task<Customer> GetCustomerOrders(string email);
        Task<List<LineItem>> GetPopularProductsByUniqueOrders();
        Task<bool> CreateCustomerAsync(Customer customer);

        Task<IEnumerable> GetCustomersAbovePriceAsync();
    }
}
