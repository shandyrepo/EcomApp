
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
    /// Сервис для работы создания заказов, пользователей и получения различной информации о заказах, покупателях, товарах из БД
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;
        public OrderService(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new NullReferenceException();
        }

        public IDbContextTransaction InitTransaction
        {
            get
            {
                return _dataContext.Database.BeginTransaction();
            }
        }
        /// <summary>
        /// Создание пользователя
        /// </summary>
        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            var existingCustomer = _dataContext.Customers.FirstOrDefault(e => e.Email == customer.Email);

            if (existingCustomer == null)
            {
                existingCustomer = new Customer { Name = customer.Name, Email = customer.Email };
                _dataContext.Customers.Add(existingCustomer);
                var save = await _dataContext.SaveChangesAsync();
                return save > 0;
            }
            return false;
        }

        /// <summary>
        /// Создание заказа для существующего пользователя с уникальным email
        /// </summary>
        public async Task<bool> CreateOrderAsync(string email, Order order)
        {
            var customer = _dataContext.Customers.FirstOrDefault(e => e.Email == email);
            customer.Orders.Add(order);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await GetCustomers().ToListAsync();
        }

        /// <summary>
        /// Получение пользавателя по email
        /// </summary>
        public async Task<Customer> GetCustomerOrders(string email)
        {
            return await GetCustomers().FirstOrDefaultAsync(p => p.Email == email);
        }

        /// <summary>
        /// Получение всех пользователей со связанными данными (заказ, позиции заказа, продукты для позиций заказа)
        /// </summary>
        private IQueryable<Customer> GetCustomers()
        {
            return _dataContext.Customers.
                Include(p => p.Orders)
                .ThenInclude(p => p.LineItems)
                .ThenInclude(p => p.Product);
        }

        /// <summary>
        /// Получение всех позиций заказов 
        /// </summary>
        public async Task<IEnumerable<LineItem>> GetAllLineItems()
        {
            return await _dataContext.LineItems.Include(e => e.Product).ToListAsync();
        }
    }
}
