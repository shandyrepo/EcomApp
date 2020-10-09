using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Models
{
    /// <summary>
    /// Информация о заказе
    /// </summary>
    public class Order
    {
        [Key]
        public int Id { get; private set; }
        public DateTime CreationDate { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
        public Order()
        {
            LineItems = new List<LineItem>();
        }
    }
}
