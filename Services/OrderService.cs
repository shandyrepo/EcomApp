
using EcomApp.Models;
using EcomApp.Data;
using EcomApp.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace EcomApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;
        public OrderService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IDbContextTransaction InitTransaction
        {
            get
            {
                return _dataContext.Database.BeginTransaction();
            }
        }
        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            var existingCustomer = _dataContext.Customers.FirstOrDefault(e => e.email == customer.email);

            if (existingCustomer == null)
            {
                existingCustomer = new Customer { name = customer.name, email = customer.email };
                _dataContext.Customers.Add(existingCustomer);
                var save = await _dataContext.SaveChangesAsync();
                return save > 0;
            }
            return false;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            try
            {
                _dataContext.Orders.Add(order);
                var created = await _dataContext.SaveChangesAsync();
                return created > 0;
            }
            catch(Exception ex)
            {
                var s = ex.Message;
            }
            return false;
        }

        public Task<IEnumerable> GetCustomersAbovePriceAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrdersByCustomer()
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetPopularProductsByUniqueOrders()
        {
            throw new NotImplementedException();
        }
    }
}
