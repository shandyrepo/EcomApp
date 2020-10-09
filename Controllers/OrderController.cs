using EcomApp.Contracts;
using EcomApp.Contracts.Request;
using EcomApp.Models;
using EcomApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcomApp.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderSevice;
        public OrderController(IOrderService orderService)
        {
            _orderSevice = orderService;
        }

        [HttpPost(ApiRoutes.Orders.Create)]
        public async Task<IActionResult> CreateOrder(string customerName, string customerEmail, [FromBody] List<OrderCreateRequest> orderDetaild)
        {
            Customer customer = new Customer { Name = customerName, Email = customerEmail };

            await _orderSevice.CreateCustomerAsync(customer);

            using (var transaction = _orderSevice.InitTransaction)
            {
                try
                {
                    Order newOrder = new Order();
                    foreach (var orderDetail in orderDetaild)
                    {
                        LineItem doubleItem = newOrder.LineItems.FirstOrDefault(e => e.ProductId == orderDetail.Id);
                        if (doubleItem == null)
                        {
                            newOrder.LineItems.Add(new LineItem()
                            {
                                ProductId = orderDetail.Id,
                                Quantity = orderDetail.Quantity
                            });
                        }
                        else
                        {
                            doubleItem.Quantity += orderDetail.Quantity;
                        }
                    }
                    var created = await _orderSevice.CreateOrderAsync(customerEmail, newOrder);

                    if (created)
                        transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return NotFound(ex.InnerException.Message);
                }
            }
            return Ok("Заказ оформлен");
        }

        [HttpGet(ApiRoutes.Orders.GetCustomerOrders)]
        public ActionResult<IEnumerable> GetCustomerOrdersByEmail(string email)
        {
            var customer = _orderSevice.GetCustomerOrders(email).Result;
            if (customer != null && customer.Orders?.Count > 0)
            {
                var res = customer.Orders.GroupBy(e => (e.LineItems, e.Id),
                    (key, value) => new
                    {
                        OrderID = key.Id,
                        TotalOrderPrice = value.Sum(e => e.LineItems.Sum(p => p.Product.Price * p.Quantity))

                    }).OrderByDescending(e => e.TotalOrderPrice);

                return Ok(res);
            }
            return NotFound("Заказы отсутствуют");
        }

        [HttpGet(ApiRoutes.Orders.GetPopularProducts)]
        public async Task<ActionResult<Product>> GetPopularProducts()
        {
            var itemsBySoldPositions = await _orderSevice.GetPopularProductsByUniqueOrders();

            var result = itemsBySoldPositions.GroupBy(product => (product.ProductId, product.Product.ProductName),
            (key, value) => new
            {
                ProductName = key.ProductName,
                TotalSold = value.Sum(item => item.Quantity)

            }).OrderByDescending(e => e.TotalSold);

            if (result.ToList().Count > 0)
                return Ok(result);
            else
                return NotFound("Отсутсвуют продукты");
        }

        [HttpGet(ApiRoutes.Orders.GetCustomersOverTotalPrice)]
        public async Task<ActionResult<IEnumerable>> GetCustomersWithTotalOrderPricesAboveValue(decimal totalOrdersPrice)
        {
            var customers = await _orderSevice.GetAllCustomersAsync();

            var result = customers.GroupBy(e => (e.Orders, e.Name),
                (key, value) => new
                {
                    Customer = key.Name,
                    Total = value.Sum(e => e.Orders.Sum(e => e.LineItems.Sum(e => e.Product.Price * e.Quantity)))

                }).OrderByDescending(e => e.Total).Where(e => e.Total > totalOrdersPrice).ToList();

            if (result.Count > 0)
                return Ok(result);
            else
                return NotFound($"Отсутсвуют клиенты с закупкой выше {totalOrdersPrice}");
        }
    }
}
