
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
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> CreateOrderAsync(string email,Order order)
        {
            try
            {
                var customer = _dataContext.Customers.FirstOrDefault(e => e.email == email);
                customer.Orders.Add(order);
                //_dataContext.Orders.Add(order);
                var created = await _dataContext.SaveChangesAsync();
                return created > 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await GetCustomers().ToListAsync();
        }

        public  async Task<Customer> GetCustomerOrders(string email)
        {
            return await GetCustomers().FirstOrDefaultAsync(p => p.email == email);
            
        }
        private IQueryable<Customer> GetCustomers()
        {
            return _dataContext.Customers.
                Include(p => p.Orders)
                .ThenInclude(p => p.LineItems)
                .ThenInclude(p => p.Product);
        }
        public async Task<List<LineItem>> GetPopularProductsByUniqueOrders()
        {
            return await _dataContext.LineItems.Include(e => e.Product).ToListAsync();
        }
    }
}
