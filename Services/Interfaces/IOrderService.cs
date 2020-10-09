using EcomApp.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace EcomApp.Services.Interfaces
{
    public interface IOrderService
    {
        IDbContextTransaction InitTransaction { get; }
        Task<bool> CreateOrderAsync(string email, Order order);
        Task<Customer> GetCustomerOrders(string email);
        Task<List<LineItem>> GetPopularProductsByUniqueOrders();
        Task<bool> CreateCustomerAsync(Customer customer);
        Task<List<Customer>> GetAllCustomersAsync();
    }
}
