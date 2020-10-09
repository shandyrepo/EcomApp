using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Contracts.Request
{
    /// <summary>
    /// Информация о заказе полученная из web-запроса <see cref="EcomApp.Contracts.Request.CustomerOrderRequest"/>
    /// </summary>
    public class CustomerOrderRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }

        public ICollection<OrderItemsRequest> Items { get; set; }

    }
  
}
