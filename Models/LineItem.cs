using System;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Models
{
    /// <summary>
    /// Позиция заказа
    /// </summary>
    public class LineItem
    {
        [Key]
        public int Id { get; private set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
