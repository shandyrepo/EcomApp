
using EcomApp.Models;
using EcomApp.Data;
using EcomApp.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcomApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;
        public OrderService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<bool> CreateOrderAsync()
        {
            throw new NotImplementedException();
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
