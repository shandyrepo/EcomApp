using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcomApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EcomApp.Contracts.Request;
using EcomApp.Models;

namespace EcomApp.Controllers
{
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
                    var created = await _orderSevice.CreateOrderAsync(newOrder);

                    if (created)
                        transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return NotFound(ex.Message);
                }

            }

            return Ok();
        }
    }
}
