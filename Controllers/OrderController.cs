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
        public async Task<IActionResult> CreateOrder(string customerName,string customerEmail,[FromBody] List<OrderCreateRequest> orderDetaild)
        {
            Customer customer = new Customer { name = customerName, email = customerEmail };

            Order newOrder = new Order();
            List<LineItem> items = new List<LineItem>();

            foreach (var orderDetail in orderDetaild)
            {
                LineItem doubleItem = items.FirstOrDefault(items => items.ProductId == orderDetail.Id);
                if (doubleItem == null)
                {
                    items.Add(new LineItem
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
            newOrder.LineItems = items;
            return  Ok();
        }
    }
}
