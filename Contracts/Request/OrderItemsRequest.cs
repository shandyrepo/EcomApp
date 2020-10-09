using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Contracts.Request
{
    /// <summary>
    /// Детали заказа для <see cref="EcomApp.Contracts.Request.CustomerOrderRequest"/> 
    /// </summary>
    public class OrderItemsRequest
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        public int Quantity { get; set; }
    }
}
