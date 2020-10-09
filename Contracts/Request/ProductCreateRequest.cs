using System;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Contracts.Request
{
    public class ProductCreateRequest
    {
        public string Name { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Цена не может быть отрицательным")]
        public decimal Price { get; set; }
    }
}
