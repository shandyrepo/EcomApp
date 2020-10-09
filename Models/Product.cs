using System;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Models
{
    /// <summary>
    /// Информация о продукте
    /// </summary>
    public class Product
    {
        [Key]
        public int Id { get; private set; }
        public string ProductName { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Цена не может быть отрицательным")]
        public decimal Price { get; set; }
    }
}
