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
using System.ComponentModel.DataAnnotations;
namespace EcomApp.Controllers
{
    /// <summary>
    /// Класс для работы с заказами покупателя и получения различной информации о заказах пользователя, популярных товарах и пр.
    /// </summary>
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderSevice;
        public OrderController(IOrderService orderService)
        {
            _orderSevice = orderService;
        }

        /// <summary>
        /// Попытка создания пользователя (если отсутствует в базе) и создание для него заказа по модели <see cref="EcomApp.Contracts.Request.CustomerOrderRequest"/>  из web-запроса  
        /// </summary>
        [HttpPost(ApiRoutes.Orders.Create)]
        public async Task<IActionResult> CreateOrder(CustomerOrderRequest customerWithOrder)
        {
            if (customerWithOrder == null)
                return NoContent();

            Customer customer = new Customer { Name = customerWithOrder.Name, Email = customerWithOrder.Email };

            await _orderSevice.CreateCustomerAsync(customer);

            using (var transaction = _orderSevice.InitTransaction)
            {
                try
                {
                    Order newOrder = new Order();
                    foreach (var orderDetail in customerWithOrder.Items)
                    {
                        LineItem doubleItem = newOrder.LineItems.FirstOrDefault(item => item.ProductId == orderDetail.Id);
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
                    bool created = await _orderSevice.CreateOrderAsync(customerWithOrder.Email, newOrder);

                    if (created)
                        transaction.Commit();
                    return Ok("Заказ оформлен");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return NotFound(ex.InnerException.Message);
                }
            }

        }

        /// <summary>
        /// Получение информации о заказах пользователя по его email-адрессу
        /// </summary>
        [HttpGet(ApiRoutes.Orders.GetCustomerOrders)]
        public ActionResult<IEnumerable> GetCustomerOrdersByEmail(string email)
        {
            var customer = _orderSevice.GetCustomerOrders(email).Result;
            if (customer != null && customer.Orders.Any())
            {
                var res = customer.Orders.GroupBy(order => (order.LineItems, order.Id),
                    (key, value) => new
                    {
                        OrderID = key.Id,
                        TotalOrderPrice = value.Sum(order => order.LineItems.Sum(item => item.Product.Price * item.Quantity))

                    }).OrderByDescending(e => e.TotalOrderPrice);

                return Ok(res);
            }
            return NotFound("Заказы отсутствуют");
        }

        /// <summary>
        /// Список проданных продуктов и их количество по уникальным заказам
        /// </summary>
        [HttpGet(ApiRoutes.Orders.GetPopularProducts)]
        public async Task<ActionResult<Product>> GetPopularProducts()
        {
            var itemsBySoldPositions = await _orderSevice.GetAllLineItems();

            var result = itemsBySoldPositions.GroupBy(product => (product.ProductId, product.Product.ProductName),
            (key, value) => new
            {
                ProductName = key.ProductName,
                TotalSold = value.Sum(item => item.Quantity)

            }).OrderByDescending(e => e.TotalSold);

            if (result.Any())
                return Ok(result);

            return NotFound("Отсутсвуют продукты");
        }

        /// <summary>
        /// Получение списка покупателей с общей стоимостью заказа превышающую заданное значение
        /// </summary>
        [HttpGet(ApiRoutes.Orders.GetCustomersOverTotalPrice)]
        public async Task<ActionResult<IEnumerable>> GetCustomersWithTotalOrderPricesAboveValue(decimal totalOrdersPrice)
        {
            var customers = await _orderSevice.GetAllCustomersAsync();

            var resultCustomers = customers.GroupBy(customer => (customer.Email, customer.Name),
                (key, value) => new
                {
                    Customer = key.Name,
                    Email = key.Email,
                    Total = value.Sum(customer => customer.Orders.Sum(order => order.LineItems.Sum(orderItem => orderItem.Product.Price * orderItem.Quantity)))

                }).OrderByDescending(result => result.Total).Where(result => result.Total > totalOrdersPrice);

            if (resultCustomers.Any())
                return Ok(resultCustomers);

            return NotFound($"Отсутсвуют клиенты с закупкой выше {totalOrdersPrice}");
        }
    }
}
