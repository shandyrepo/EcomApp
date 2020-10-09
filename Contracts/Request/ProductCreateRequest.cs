using System;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Contracts.Request
{
    /// <summary>
    /// Информация для создания продукта через web-запрос
    /// </summary>
    public class ProductCreateRequest
    {
        public string Name { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Цена не может быть отрицательным")]
        public decimal Price { get; set; }
    }
}
