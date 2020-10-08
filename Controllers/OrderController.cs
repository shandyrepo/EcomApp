using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using EcomApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EcomApp.Contracts.Request;
using EcomApp.Models;

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

        [HttpPost("/CreateOrder")]
        public async Task<IActionResult> CreateOrder(string customerName, string customerEmail, [FromBody] List<OrderCreateRequest> orderDetaild)
        {
            Customer customer = new Customer { name = customerName, email = customerEmail };

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
                                quantity = orderDetail.Quantity
                            });
                        }
                        else
                        {
                            doubleItem.quantity += orderDetail.Quantity;
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

            return Ok();
        }


        [HttpGet("/GetCustomerOrders")]
        public async Task<ActionResult<IEnumerable>> GetCustomerOrdersByEmail(string email)
        {

            var customer = _orderSevice.GetCustomerOrders(email).Result;
            if (customer != null && customer.Orders?.Count > 0) {
                var res = customer.Orders.GroupBy(e => (e.LineItems, e.id),
                    (key, value) => new
                    {
                        OrderID = key.id,
                        TotalOrderPrice = value.Sum(e => e.LineItems.Sum(p => p.Product.price * p.quantity))

                    }).OrderByDescending(e => e.TotalOrderPrice);

                return Ok(res);
            }

            return NotFound("Заказы отсутствуют");

        }

        [HttpGet("/GetPopularProducts")]
        public async Task<ActionResult<Product>> GetPopularProducts()
        {
            var itemsBySoldPositions = await _orderSevice.GetPopularProductsByUniqueOrders();

            var result = itemsBySoldPositions.GroupBy(product => (product.ProductId, product.Product.productName),
            (key, value) => new
            {
                ProductName = key.productName,
                TotalSold = value.Sum(item => item.quantity)
            }).OrderByDescending(e => e.TotalSold);

            if (result.ToList().Count > 0)
                return Ok(result);
            else 
                return NotFound("Отсутсвуют продукты");
        }


        [HttpGet("/GetCustomersWithTotalOrdersPriceAboveValue")]
        public async Task<ActionResult<IEnumerable>> GetCustomersWithTotalOrderPricesAboveValue(decimal totalOrdersPrice)
        {
            var customers = await _orderSevice.GetAllCustomersAsync();

            var result = customers.GroupBy(e => (e.Orders, e.name),
                (key, value) => new
                {
                    Customer = key.name,
                    Total = value.Sum(e => e.Orders.Sum(e => e.LineItems.Sum(e => e.Product.price * e.quantity)))

                }).OrderByDescending(e => e.Total).Where(e => e.Total > totalOrdersPrice);

            if (result.ToList().Count > 0)
                return Ok(result);
            else
                return NotFound($"Отсутсвуют клиенты с закупкой выше {totalOrdersPrice}");
        }
    }
}
